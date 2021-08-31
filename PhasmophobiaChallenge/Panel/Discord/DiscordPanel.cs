using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using PhasmophobiaChallenge.Settings;
using PhasmophobiaChallenge.Settings.PropertiesControls;
using System.Threading;

namespace PhasmophobiaChallenge.Panel.Discord
{
    public partial class DiscordPanel : APhasmophobiaCompanionPanel
    {
        private bool m_IsLoading = false;
        private bool m_BotReady = false;
        private ulong m_BotID = 806130388622770198;
        private Mutex m_DiscordMutex = new Mutex();
        private readonly List<Challenge> m_Challenges = new List<Challenge>();
        private readonly Dictionary<ulong, DiscordMember> m_Members = new Dictionary<ulong, DiscordMember>();
        private readonly Dictionary<ulong, DiscordChannel> m_Channels = new Dictionary<ulong, DiscordChannel>();
        private DiscordMember m_SelectedMember = null;
        private DiscordChannel m_ChallengeChannel = null;
        private DiscordChannel m_WelcomeChannel = null;
        private DiscordGuild m_Guild = null;
        private DiscordClient m_DiscordClient = null;

        public DiscordPanel(MainWindow mainWindow): base(mainWindow, EPanelType.Invalid/*EPanelType.DiscordChallenges*/, "panel.discord")
        {
            InitializeComponent();
        }

        public override void OnOpen()
        {
            PanelUIManager.RegisterImageButton(SettingsButton, Properties.Resources.tv_remote_icon, Properties.Resources.tv_remote_icon_over, Properties.Resources.tv_remote_icon_clicked);
            PanelUIManager.RegisterImageButton(BackButton, Properties.Resources.red_arrow, Properties.Resources.red_arrow_over, Properties.Resources.red_arrow_clicked);
            m_DiscordClient = new DiscordClient(new DiscordConfiguration()
            {
                Token = GetData().Get<string>("token"),
                TokenType = TokenType.Bot
            });
            m_DiscordClient.GuildDownloadCompleted += OnGuildDownloadCompleted;
            m_DiscordClient.GuildAvailable += OnGuildAvailable;
            m_DiscordClient.ConnectAsync().Wait();
        }

        public override void OnClose()
        {
            if (m_DiscordClient != null)
                m_DiscordClient.DisconnectAsync().Wait();
            m_BotReady = false;
        }

        delegate void SetIsLoadingCallback(bool isLoading);
        private void SetIsLoading(bool isLoading)
        {
            if (WaitingAnimation.InvokeRequired)
            {
                SetIsLoadingCallback callback = new SetIsLoadingCallback(SetIsLoading);
                Invoke(callback, new object[] { isLoading });
            }
            else
            {
                m_IsLoading = isLoading;
                WaitingAnimation.Visible = isLoading;
            }
        }

        delegate void UpdateChallengesCallback();
        private void UpdateChallenges()
        {
            if (WaitingAnimation.InvokeRequired)
            {
                UpdateChallengesCallback callback = new UpdateChallengesCallback(UpdateChallenges);
                Invoke(callback, new object[] {});
            }
            else
            {
                FormFlowLayoutPanel.Controls.Clear();
                foreach (Challenge challenge in m_Challenges)
                {
                    Console.Write("Author: ");
                    Console.WriteLine(challenge.GetAuthor());
                    Console.Write("Name: ");
                    Console.WriteLine(challenge.GetName());
                    Console.Write("Content: ");
                    Console.WriteLine(challenge.GetDescription());
                }
            }
        }

        static T Synchronize<T>(Task<T> task)
        {
            task.Wait();
            return task.Result;
        }

        static void Synchronize(Task task)
        {
            task.Wait();
        }

        private async Task OnGuildDownloadCompleted(DiscordClient client, GuildDownloadCompletedEventArgs args)
        {
            m_DiscordMutex.WaitOne();
            OnBotReady();
            m_DiscordMutex.ReleaseMutex();
        }

        private async Task OnGuildAvailable(DiscordClient client, GuildCreateEventArgs args)
        {
            m_DiscordMutex.WaitOne();
            OnBotReady();
            m_DiscordMutex.ReleaseMutex();
        }

        private void OnBotReady()
        {
            if (m_BotReady || m_IsLoading)
                return;
            ReloadGuild();
            m_BotReady = true;
        }

        private void ReloadGuild()
        {
            SetIsLoading(true);
            m_Channels.Clear();
            DataFragment data = GetData();
            m_Guild = Synchronize(m_DiscordClient.GetGuildAsync(data.Get<ulong>("serverID")));
            IReadOnlyList <DiscordChannel> channels = Synchronize(m_Guild.GetChannelsAsync());
            ulong challengeChannelID = data.Get<ulong>("challengeChannelID");
            ulong welcomeChannelID = data.Get<ulong>("welcomeChannelID");
            foreach (DiscordChannel channel in channels)
            {
                if (channel.Id == challengeChannelID)
                    m_ChallengeChannel = channel;
                if (channel.Id == welcomeChannelID)
                    m_WelcomeChannel = channel;
                if (!channel.IsCategory)
                    m_Channels[channel.Id] = channel;
            }
            ReloadMembers();
            SetIsLoading(false);
        }

        private void ReloadMembers()
        {
            if (m_WelcomeChannel != null)
            {
                m_Members.Clear();
                ulong id = m_Guild.Owner.Id;
                ulong currentMemberID = GetData().Get<ulong>("currentMemberID");
                DiscordMember member = Synchronize(m_Guild.GetMemberAsync(id));
                if (member != null)
                {
                    if (id == currentMemberID)
                        m_SelectedMember = member;
                    if (!member.IsBot)
                        m_Members[id] = member;
                }
                Console.WriteLine("Searching for member with ID " + currentMemberID);
                IReadOnlyList <DiscordMessage> messages = Synchronize(m_WelcomeChannel.GetMessagesAsync());
                foreach (DiscordMessage message in messages)
                {
                    if (message.MessageType == MessageType.GuildMemberJoin)
                    {
                        id = message.Author.Id;
                        member = Synchronize(m_Guild.GetMemberAsync(id));
                        if (member != null)
                        {
                            if (id == currentMemberID)
                                m_SelectedMember = member;
                            if (!member.IsBot)
                                m_Members[id] = member;
                        }
                    }
                }
                ReloadChallenges();
            }
        }

        private void ReloadChallenges()
        {
            if (m_ChallengeChannel != null && m_SelectedMember != null)
            {
                Console.WriteLine("Searching for challenges of " + m_SelectedMember.DisplayName);
                IReadOnlyList<DiscordMessage> messages = Synchronize(m_ChallengeChannel.GetMessagesAsync());
                foreach (DiscordMessage message in messages)
                {
                    ulong authorId = message.Author.Id;
                    DiscordMember author = null;
                    if (m_Members.ContainsKey(authorId))
                        author = m_Members[authorId];
                    Challenge challenge = new Challenge(message, author, m_BotID);
                    if (challenge.GetAuthorId() == m_SelectedMember.Id)
                        m_Challenges.Add(challenge);
                }
                UpdateChallenges();
            }
        }

        private void SendChallenge(string name, string description)
        {
            DiscordMessage message = Synchronize(m_ChallengeChannel.SendMessageAsync("*Proposé par *" + m_SelectedMember.Nickname + "\n**" + name + "**\n" + description));
            var yesEmoji = DiscordEmoji.FromName(m_DiscordClient, ":arrow_up:");
            var noEmoji = DiscordEmoji.FromName(m_DiscordClient, ":arrow_down:");
            Synchronize(message.CreateReactionAsync(yesEmoji));
            Synchronize(message.CreateReactionAsync(noEmoji));
            Challenge challenge = new Challenge(m_SelectedMember.Nickname, name, description, message.Id, true);
            m_Challenges.Add(challenge);
            UpdateChallenges();
        }

        private void UpdateChallenge(Challenge challenge)
        {
            DiscordMessage message = Synchronize(m_ChallengeChannel.GetMessageAsync(challenge.GetId()));
            Synchronize(message.ModifyAsync(challenge.GetMessage()));
            UpdateChallenges();
        }

        private void DeleteChallenge(Challenge challenge)
        {
            DiscordMessage message = Synchronize(m_ChallengeChannel.GetMessageAsync(challenge.GetId()));
            Synchronize(message.DeleteAsync());
            m_Challenges.Remove(challenge);
            UpdateChallenges();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            GetMainWindow().SetPanel(EPanelType.TitleScreen);
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            //if (!m_IsLoading)
            //{
                PropertiesControlFactory factory = new PropertiesControlFactory();
                factory.AddBuilder("Token", new PrivatePropertyControlBuilder());
                MemberPropertyControlBuilder memberBuilder = new MemberPropertyControlBuilder();
                foreach (DiscordMember member in m_Members.Values)
                    memberBuilder.AddMember(member);
                factory.AddBuilder("CurrentMemberID", memberBuilder);
                ChannelPropertyControlBuilder channelBuilder = new ChannelPropertyControlBuilder();
                foreach (DiscordChannel channel in m_Channels.Values)
                    channelBuilder.AddChannel(channel);
                factory.AddBuilder("ChallengeChannelID", channelBuilder);
                factory.AddBuilder("WelcomeChannelID", channelBuilder);

                SettingsForm settingsForm = new SettingsForm(GetData(), factory);
                settingsForm.OnValidateForm += ReloadGuild;
                settingsForm.Show();
            //}
        }
    }
}
