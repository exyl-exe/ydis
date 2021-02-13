using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ydis.Properties;

namespace Ydis.ViewModels.CurrentLevel
{
    /// <summary>
    /// View model for the placeholder
    /// </summary>
    public class CurrentLevelPlaceholderViewModel : BaseViewModel
    {
        /// <summary>
        /// Text to display as a header
        /// </summary>
        public string TitleText => Resources.CurrentLevelNotPlayingTitle;
        /// <summary>
        /// Additional text that displays information about the software
        /// </summary>
        public string BodyText => Resources.CurrentLevelNotPlayingText;
        /// <summary>
        /// Github part label
        /// </summary>
        public string GithubText => Resources.GithubLabel;
        /// <summary>
        /// Releases link
        /// </summary>
        public string GithubContentText => Resources.GithubLink;

        public string DiscordText => Resources.DiscordLabel;
        public string DiscordLink => Resources.DiscordLink;

        /// <summary>
        /// Footer content (author + version)
        /// </summary>
        public string Footer => string.Format(Resources.AuthorFormat, Resources.Version);

        public CurrentLevelPlaceholderViewModel() { }
    }
}
