// Copyright (C) 2014-2019, GitHub/Codeplex user AlphaCentaury
// 
// All rights reserved, except those granted by the governing license of this software.
// See 'license.txt' file in the project root for complete license information.
// 
// http://www.alphacentaury.org/movistartv https://github.com/AlphaCentaury

using System;
using System.ComponentModel;
using System.Windows.Forms;
using IpTviewr.Common;
using IpTviewr.UiServices.Discovery;
using IpTviewr.Services.EpgDiscovery;

namespace IpTviewr.UiServices.EPG
{
    public partial class EpgProgramMiniBar : UserControl
    {
        private UiBroadcastService _service;
        private EpgProgram _epgProgram;

        public EpgProgramMiniBar()
        {
            InitializeComponent();
        } // constructor

        public void DisplayData(UiBroadcastService service, EpgProgram epgProgram, DateTime referenceTime, string caption)
        {
            _service = service;
            _epgProgram = epgProgram;

            labelProgramCaption.Text = caption;
            labelProgramCaption.Visible = caption != null;
            labelProgramTime.Visible = (epgProgram != null);
            labelProgramDetails.Visible = (epgProgram != null);
            pictureProgramThumbnail.ImageLocation = null;
            buttonProgramProperties.Visible = (epgProgram != null);
            pictureProgramThumbnail.Cursor = (epgProgram != null) ? Cursors.Hand : Cursors.Default;

            if (_epgProgram == null)
            {
                labelProgramTitle.Text = Properties.Texts.EpgNoInformation;
                pictureProgramThumbnail.Image = Properties.Resources.EpgNoProgramImage;
            }
            else
            {
                labelProgramTitle.Text = _epgProgram.Title;
                labelProgramTime.Text = string.Format("{0} ({1})", FormatString.DateTimeFromToMinutes(_epgProgram.LocalStartTime, _epgProgram.LocalEndTime, referenceTime),
                    FormatString.TimeSpanTotalMinutes(_epgProgram.Duration, FormatString.Format.Extended));
                labelProgramDetails.Text = string.Format("{0} / {1}", (_epgProgram.Genre != null) ? _epgProgram.Genre.Description : Properties.Texts.EpgNoGenre,
                    (_epgProgram.ParentalRating != null) ? _epgProgram.ParentalRating.Description : Properties.Texts.EpgNoParentalRating);

                pictureProgramThumbnail.Image = Properties.Resources.EpgLoadingProgramImage;
                pictureProgramThumbnail.ImageLocation = null;
                // TODO: EPG
                // pictureProgramThumbnail.ImageLocation = IpTvProvider.Current.EpgInfo.GetEpgProgramThumbnailUrl(service, EpgProgram, false);
            } // if-else
        }  // DisplayData

        private void buttonProgramProperties_Click(object sender, EventArgs e)
        {
            EpgExtendedInfoDialog.ShowExtendedInfo(this, _service, _epgProgram);
        } // buttonProgramProperties_Click

        private void pictureProgramThumbnail_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if ((e.Error != null) || (e.Cancelled))
            {
                (sender as PictureBox).Image = Properties.Resources.EpgNoProgramImage;
            } // if
        } // pictureProgramThumbnail_LoadCompleted

        private void pictureProgramThumbnail_Click(object sender, EventArgs e)
        {
            EpgExtendedInfoDialog.ShowExtendedInfo(this, _service, _epgProgram);
        } // pictureProgramThumbnail_Click
    } // class EpgProgramMiniBar
} // namespace
