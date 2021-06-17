using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PhasmophobiaChallenge
{
    public enum EString
    {
        //Panel titles
        TitleScreen,
        Speedrun,
        RandomStuff,
        Discord,
        Dummy,
        Option,
        StoryMode,
        //Other
        AppTitle,
        Randomize,
        Reset,
        OutOfItem,
        Back,
        Money,
        Job,
        Traits,
        Inventory,
        NewProfile,
        EnterProfileName,
        ProfileName,
        EnterMoneyToAdd,
        MoneyToAdd,
        EnterMoneyToPay,
        MoneyToPay,
        AreYouSure,
        ConfirmDeleteProfile,
        Yes,
        No,
        Version,
        Exit,
        //Ghost types
        Spirit,
        Wraith,
        Phantom,
        Poltergeist,
        Banshee,
        Jinn,
        Mare,
        Revenant,
        Shade,
        Demon,
        Yurei,
        Oni,
        Yokai,
        Hantu,
        //Ghost evidences
        Freezing,
        GhostWriting,
        GhostOrb,
        Fingerprints,
        SpiritBox,
        EMF5,
        //Items
        EMFReader,
        Flashlight,
        StrongFlashlight,
        PhotoCamera,
        Lighter,
        Candle,
        UVLight,
        Crucifix,
        VideoCamera,
        SpiritBoxItem,
        Salt,
        SmudgeSticks,
        Tripod,
        MotionSensor,
        SoundSensor,
        SanityPills,
        Thermometer,
        GhostWritingBook,
        InfraredLightSensor,
        ParabolicMicrophone,
        Glowstick,
        HeadMountedCamera,
        //Jobs
        Archaeologist,
        TeenageMovieActor,
        Unemployed,
        BabySitter,
        Speedrunner,
        Podcaster,
        Cameraman,
        Doctor,
        //Traits
        Sensitive,
        Technophobe,
        Dwarf,
        Bold,
        Chilly,
        ButterFingers,
        Courteous,
        Coward,
        Deaf,
        Confuse,
        Alzheimer,
        Germophobe,
        Cardiac,
        Asthmatic,
        Nycotophobic,
        Giant,
        Paranoide,
        Attentive,
        Conspiratorial,
        Vampire,
        Teenage,
        Hurry,
        Shy,
        SmallPocket,
        Fussy,
        Inattentive,
        WeakBladder,
        //\\
        Count,
        Invalid
    }

    public enum ELocal
    {
        English,
        French
    }

    public class Translator
    {
        private class Local
        {
            private Dictionary<EString, string> m_Translations = new Dictionary<EString, string>();

            public string GetString(EString text)
            {
                if (m_Translations.TryGetValue(text, out string ret))
                    return ret;
                return null;
            }

            public void AddTranslation(EString text, string translation)
            {
                m_Translations.Add(text, translation);
            }
        }

        private readonly DataFragment m_Data;
        private List<KeyValuePair<EString, Control>> m_Controls =  new List<KeyValuePair<EString, Control>>();
        private Dictionary<ELocal, Local> m_Translators = new Dictionary<ELocal, Local>();
        private ELocal[] m_Locals = Enum.GetValues(typeof(ELocal)).Cast<ELocal>().ToArray();
        private uint m_Current = 0;

        public Translator(DataFragment data)
        {
            m_Data = data;
            if (m_Data.Find("local"))
                m_Current = m_Data.Get<uint>("local");

            {
                Local local = new Local();
                //Panel titles
                local.AddTranslation(EString.TitleScreen, "Écran Titre");
                local.AddTranslation(EString.Speedrun, "Speedrun");
                local.AddTranslation(EString.RandomStuff, "Matériel aléatoire");
                local.AddTranslation(EString.Discord, "Compagnon Discord");
                local.AddTranslation(EString.Dummy, "Dummy");
                local.AddTranslation(EString.Option, "Options");
                local.AddTranslation(EString.StoryMode, "Mode Histoire");

                //Other
                local.AddTranslation(EString.AppTitle, "Compagnon Phasmophobia");
                local.AddTranslation(EString.Randomize, "Aléatoire");
                local.AddTranslation(EString.Reset, "Réinitialiser");
                local.AddTranslation(EString.OutOfItem, "Plus d'objets");
                local.AddTranslation(EString.Back, "Retour");
                local.AddTranslation(EString.Money, "Argent");
                local.AddTranslation(EString.Job, "Métier");
                local.AddTranslation(EString.Traits, "Traits");
                local.AddTranslation(EString.Inventory, "Inventaire");
                local.AddTranslation(EString.NewProfile, "Nouveau Profile");
                local.AddTranslation(EString.EnterProfileName, "Choisissez un nom de profile");
                local.AddTranslation(EString.ProfileName, "Nom de profile:");
                local.AddTranslation(EString.EnterMoneyToAdd, "Choisissez la somme à ajouter");
                local.AddTranslation(EString.MoneyToAdd, "Somme à ajouter");
                local.AddTranslation(EString.EnterMoneyToPay, "Choisissez la somme à payer");
                local.AddTranslation(EString.MoneyToPay, "Somme à payer");
                local.AddTranslation(EString.AreYouSure, "Êtes vous sûre?");
                local.AddTranslation(EString.ConfirmDeleteProfile, "Voulez-vous supprimer ce profil?");
                local.AddTranslation(EString.Yes, "Oui");
                local.AddTranslation(EString.No, "Non");
                local.AddTranslation(EString.Version, "Version");
                local.AddTranslation(EString.Exit, "Quitter");

                //Ghost types
                local.AddTranslation(EString.Spirit, "Esprit");
                local.AddTranslation(EString.Wraith, "Spectre");
                local.AddTranslation(EString.Phantom, "Fantôme");
                local.AddTranslation(EString.Poltergeist, "Poltergeist");
                local.AddTranslation(EString.Banshee, "Banshee");
                local.AddTranslation(EString.Jinn, "Jinn");
                local.AddTranslation(EString.Mare, "Cauchemar");
                local.AddTranslation(EString.Revenant, "Revenant");
                local.AddTranslation(EString.Shade, "Ombre");
                local.AddTranslation(EString.Demon, "Demon");
                local.AddTranslation(EString.Yurei, "Yurei");
                local.AddTranslation(EString.Oni, "Oni");
                local.AddTranslation(EString.Yokai, "Yokai");
                local.AddTranslation(EString.Hantu, "Hantu");

                //Ghost evidences
                local.AddTranslation(EString.EMF5, "EMF 5");
                local.AddTranslation(EString.SpiritBox, "Spirit Box");
                local.AddTranslation(EString.Fingerprints, "Empreintes");
                local.AddTranslation(EString.GhostOrb, "Orbes fantômatiques");
                local.AddTranslation(EString.GhostWriting, "Écritures fantômatiques");
                local.AddTranslation(EString.Freezing, "Températures glaciales");

                //Items
                local.AddTranslation(EString.EMFReader, "Lecteur EMF");
                local.AddTranslation(EString.Flashlight, "Lampe");
                local.AddTranslation(EString.StrongFlashlight, "Lampe puissante");
                local.AddTranslation(EString.PhotoCamera, "Appareil photo");
                local.AddTranslation(EString.Lighter, "Briquet");
                local.AddTranslation(EString.Candle, "Bougie");
                local.AddTranslation(EString.UVLight, "Lumière UV");
                local.AddTranslation(EString.Crucifix, "Crucifix");
                local.AddTranslation(EString.VideoCamera, "Caméra Vidéo");
                local.AddTranslation(EString.SpiritBoxItem, "Spirit Box");
                local.AddTranslation(EString.Salt, "Sel");
                local.AddTranslation(EString.SmudgeSticks, "Bâton d'encens");
                local.AddTranslation(EString.Tripod, "Trépieds");
                local.AddTranslation(EString.MotionSensor, "Capteur de mouvement");
                local.AddTranslation(EString.SoundSensor, "Capteur sonore");
                local.AddTranslation(EString.SanityPills, "Pilules calmantes");
                local.AddTranslation(EString.Thermometer, "Thermomètre");
                local.AddTranslation(EString.GhostWritingBook, "Livre d'écriture fantomatique");
                local.AddTranslation(EString.InfraredLightSensor, "Capteur infrarouge");
                local.AddTranslation(EString.ParabolicMicrophone, "Microphone parabolique");
                local.AddTranslation(EString.Glowstick, "Bâton lumineux");
                local.AddTranslation(EString.HeadMountedCamera, "Caméra frontale");

                //Jobs
                local.AddTranslation(EString.Archaeologist, "Archéologue");
                local.AddTranslation(EString.TeenageMovieActor, "Acteur de teenage movie");
                local.AddTranslation(EString.Unemployed, "Chômeur");
                local.AddTranslation(EString.BabySitter, "Assistant maternel");
                local.AddTranslation(EString.Speedrunner, "Speedrunner");
                local.AddTranslation(EString.Podcaster, "Animateur radio");
                local.AddTranslation(EString.Cameraman, "Caméraman");
                local.AddTranslation(EString.Doctor, "Médecin");

                //Traits
                local.AddTranslation(EString.Sensitive, "Odorat sensible");
                local.AddTranslation(EString.Technophobe, "Technophobe");
                local.AddTranslation(EString.Dwarf, "Nain");
                local.AddTranslation(EString.Bold, "Courageux");
                local.AddTranslation(EString.Chilly, "Frileux");
                local.AddTranslation(EString.ButterFingers, "Main de beurre");
                local.AddTranslation(EString.Courteous, "Courtois");
                local.AddTranslation(EString.Coward, "Peureux");
                local.AddTranslation(EString.Deaf, "Sourd");
                local.AddTranslation(EString.Confuse, "Confus");
                local.AddTranslation(EString.Alzheimer, "Alzheimer");
                local.AddTranslation(EString.Germophobe, "Germaphobe");
                local.AddTranslation(EString.Cardiac, "Cardiaque");
                local.AddTranslation(EString.Asthmatic, "Asthmatique");
                local.AddTranslation(EString.Nycotophobic, "Peur du noir");
                local.AddTranslation(EString.Giant, "Géant");
                local.AddTranslation(EString.Paranoide, "Paranoïaque");
                local.AddTranslation(EString.Attentive, "Attentif");
                local.AddTranslation(EString.Conspiratorial, "Complotiste");
                local.AddTranslation(EString.Vampire, "Vampire");
                local.AddTranslation(EString.Teenage, "Adolescent");
                local.AddTranslation(EString.Hurry, "Pressé");
                local.AddTranslation(EString.Shy, "Timide");
                local.AddTranslation(EString.SmallPocket, "En jean");
                local.AddTranslation(EString.Fussy, "Maniaque");
                local.AddTranslation(EString.Inattentive, "Tête en l'air");
                local.AddTranslation(EString.WeakBladder, "Pisse-minute");
                m_Translators.Add(ELocal.French, local);
            }

            {
                Local local = new Local();
                //Panel titles
                local.AddTranslation(EString.TitleScreen, "Title Screen");
                local.AddTranslation(EString.Speedrun, "Speedrun");
                local.AddTranslation(EString.RandomStuff, "Random Stuff");
                local.AddTranslation(EString.Discord, "Discord companion");
                local.AddTranslation(EString.Dummy, "Dummy");
                local.AddTranslation(EString.Option, "Options");
                local.AddTranslation(EString.StoryMode, "Story Mode");

                //Other
                local.AddTranslation(EString.AppTitle, "Phasmophobia Companion");
                local.AddTranslation(EString.Randomize, "Randomize");
                local.AddTranslation(EString.Reset, "Reset");
                local.AddTranslation(EString.OutOfItem, "Out of items");
                local.AddTranslation(EString.Back, "Back");
                local.AddTranslation(EString.Money, "Money");
                local.AddTranslation(EString.Job, "Job");
                local.AddTranslation(EString.Traits, "Traits");
                local.AddTranslation(EString.Inventory, "Inventory");
                local.AddTranslation(EString.NewProfile, "New Profile");
                local.AddTranslation(EString.EnterProfileName, "Enter a profile name");
                local.AddTranslation(EString.ProfileName, "Profile name:");
                local.AddTranslation(EString.EnterMoneyToAdd, "Enter the money to add");
                local.AddTranslation(EString.MoneyToAdd, "Money to add");
                local.AddTranslation(EString.EnterMoneyToPay, "Enter the money to pay");
                local.AddTranslation(EString.MoneyToPay, "Money to pay");
                local.AddTranslation(EString.AreYouSure, "Are you sure?");
                local.AddTranslation(EString.ConfirmDeleteProfile, "Do you want to delete this profile?");
                local.AddTranslation(EString.Yes, "Yes");
                local.AddTranslation(EString.No, "No");
                local.AddTranslation(EString.Version, "Version");
                local.AddTranslation(EString.Exit, "Exit");

                //Ghost types
                local.AddTranslation(EString.Spirit, "Spirit");
                local.AddTranslation(EString.Wraith, "Wraith");
                local.AddTranslation(EString.Phantom, "Phantom");
                local.AddTranslation(EString.Poltergeist, "Poltergeist");
                local.AddTranslation(EString.Banshee, "Banshee");
                local.AddTranslation(EString.Jinn, "Jinn");
                local.AddTranslation(EString.Mare, "Mare");
                local.AddTranslation(EString.Revenant, "Revenant");
                local.AddTranslation(EString.Shade, "Shade");
                local.AddTranslation(EString.Demon, "Demon");
                local.AddTranslation(EString.Yurei, "Yurei");
                local.AddTranslation(EString.Oni, "Oni");
                local.AddTranslation(EString.Yokai, "Yokai");
                local.AddTranslation(EString.Hantu, "Hantu");

                //Ghost evidences
                local.AddTranslation(EString.EMF5, "EMF 5");
                local.AddTranslation(EString.SpiritBox, "Spirit Box");
                local.AddTranslation(EString.Fingerprints, "Fingerprints");
                local.AddTranslation(EString.GhostOrb, "Ghost Orb");
                local.AddTranslation(EString.GhostWriting, "Ghost Writing");
                local.AddTranslation(EString.Freezing, "Freezing Temperatures");

                //Items
                local.AddTranslation(EString.EMFReader, "EMF Reader");
                local.AddTranslation(EString.Flashlight, "Flashlight");
                local.AddTranslation(EString.StrongFlashlight, "Strong flashlight");
                local.AddTranslation(EString.PhotoCamera, "Photo camera");
                local.AddTranslation(EString.Lighter, "Lighter");
                local.AddTranslation(EString.Candle, "Candle");
                local.AddTranslation(EString.UVLight, "UV Light");
                local.AddTranslation(EString.Crucifix, "Crucifix");
                local.AddTranslation(EString.VideoCamera, "Video Camera");
                local.AddTranslation(EString.SpiritBoxItem, "Spirit Box");
                local.AddTranslation(EString.Salt, "Salt");
                local.AddTranslation(EString.SmudgeSticks, "Smudge Sticks");
                local.AddTranslation(EString.Tripod, "Tripod");
                local.AddTranslation(EString.MotionSensor, "Motion Sensor");
                local.AddTranslation(EString.SoundSensor, "Sound Sensor");
                local.AddTranslation(EString.SanityPills, "Sanity Pills");
                local.AddTranslation(EString.Thermometer, "Thermometer");
                local.AddTranslation(EString.GhostWritingBook, "Ghost Writing Book");
                local.AddTranslation(EString.InfraredLightSensor, "Infrared Light Sensor");
                local.AddTranslation(EString.ParabolicMicrophone, "Parabolic Microphone");
                local.AddTranslation(EString.Glowstick, "Glowstick");
                local.AddTranslation(EString.HeadMountedCamera, "Head Mounted Camera");

                //Jobs
                local.AddTranslation(EString.Archaeologist, "Archaeologist");
                local.AddTranslation(EString.TeenageMovieActor, "Teenage movie actor");
                local.AddTranslation(EString.Unemployed, "Unemployed");
                local.AddTranslation(EString.BabySitter, "Baby-sitter");
                local.AddTranslation(EString.Speedrunner, "Speedrunner");
                local.AddTranslation(EString.Podcaster, "Podcaster");
                local.AddTranslation(EString.Cameraman, "Cameraman");
                local.AddTranslation(EString.Doctor, "Doctor");

                //Traits
                local.AddTranslation(EString.Sensitive, "Sensitive");
                local.AddTranslation(EString.Technophobe, "Technophobe");
                local.AddTranslation(EString.Dwarf, "Dwarf");
                local.AddTranslation(EString.Bold, "Bold");
                local.AddTranslation(EString.Chilly, "Chilly");
                local.AddTranslation(EString.ButterFingers, "Butter fingers");
                local.AddTranslation(EString.Courteous, "Courteous");
                local.AddTranslation(EString.Coward, "Coward");
                local.AddTranslation(EString.Deaf, "Deaf");
                local.AddTranslation(EString.Confuse, "Confuse");
                local.AddTranslation(EString.Alzheimer, "Alzheimer");
                local.AddTranslation(EString.Germophobe, "Germophobe");
                local.AddTranslation(EString.Cardiac, "Cardiac");
                local.AddTranslation(EString.Asthmatic, "Asthmatic");
                local.AddTranslation(EString.Nycotophobic, "Nycotophobic");
                local.AddTranslation(EString.Giant, "Giant");
                local.AddTranslation(EString.Paranoide, "Paranoide");
                local.AddTranslation(EString.Attentive, "Attentive");
                local.AddTranslation(EString.Conspiratorial, "Conspiratorial");
                local.AddTranslation(EString.Vampire, "Vampire");
                local.AddTranslation(EString.Teenage, "Teenage");
                local.AddTranslation(EString.Hurry, "Hurry");
                local.AddTranslation(EString.Shy, "Shy");
                local.AddTranslation(EString.SmallPocket, "Small pocket");
                local.AddTranslation(EString.Fussy, "Fussy");
                local.AddTranslation(EString.Inattentive, "Inattentive");
                local.AddTranslation(EString.WeakBladder, "Weak bladder");
                m_Translators.Add(ELocal.English, local);
            }
        }

        public void RegisterGlobalControl(EString text, Control control)
        {
            foreach (KeyValuePair<EString, Control> pair in m_Controls)
            {
                if (pair.Value == control)
                    return;
            }
            control.Text = GetString(text);
            m_Controls.Add(new KeyValuePair<EString, Control>(text, control));
        }

        public void RegisterControl(EString text, Control control)
        {
            foreach (KeyValuePair<EString, Control> pair in m_Controls)
            {
                if (pair.Value == control)
                    return;
            }
            control.Text = GetString(text);
            m_Controls.Add(new KeyValuePair<EString, Control>(text, control));
        }

        public void UnregisterControls()
        {
            m_Controls.Clear();
        }

        public string GetString(EString text)
        {
            if (m_Translators.TryGetValue(m_Locals[m_Current], out Local local))
                return local.GetString(text);
            return null;
        }

        public string GetLocalName()
        {
            switch (m_Locals[m_Current])
            {
                case ELocal.English:
                    return "English";
                case ELocal.French:
                    return "Français";
            }
            return null;
        }

        public void NextLocal()
        {
            ++m_Current;
            if (m_Current == m_Locals.Length)
                m_Current = 0;
            m_Data.Set("local", m_Current);
            m_Data.Save();
            Update();
        }

        public void PreviousLocal()
        {
            if (m_Current == 0)
                m_Current = (uint)(m_Locals.Length - 1);
            else
                --m_Current;
            m_Data.Set("local", m_Current);
            m_Data.Save();
            Update();
        }

        private void Update()
        {
            foreach (KeyValuePair<EString, Control> pair in m_Controls)
                pair.Value.Text = GetString(pair.Key);
        }
    }
}
