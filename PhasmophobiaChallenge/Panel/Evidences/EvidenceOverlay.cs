using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Panel.Evidences
{
    public partial class EvidenceOverlay : UserControl
    {
        private ResourceManager m_ResourcesManager;
        private readonly Dictionary<uint, string> m_EvidenceToImage = new Dictionary<uint, string>();

        public EvidenceOverlay()
        {
            InitializeComponent();
        }

        internal void Update(HashSet<uint> foundEvidences, List<Ghost> possibleGhosts)
        {
            Controls.Clear();
            int pictureBoxX = 5;
            foreach (uint evidence in foundEvidences)
            {
                string imageName = m_EvidenceToImage[evidence];
                PictureBox pictureBox = new PictureBox();
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Image = m_ResourcesManager.GetObject(imageName) as Image;
                pictureBox.Size = new Size(75, 75);
                pictureBox.Location = new Point(pictureBoxX, 5);
                pictureBoxX += 80;
                Controls.Add(pictureBox);
            }
        }

        internal void OnOpen()
        {
            m_ResourcesManager = new ResourceManager("PhasmophobiaChallenge.Properties.Resources", Assembly.GetExecutingAssembly());
        }

        internal void OnClose()
        {
            m_ResourcesManager.ReleaseAllResources();
        }

        internal void RegisterEvidenceImage(uint evidence, string image)
        {
            m_EvidenceToImage[evidence] = image;
        }
    }
}
