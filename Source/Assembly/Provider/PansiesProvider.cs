using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Text;
using CodeOwls.PowerShell.Paths.Processors;
using CodeOwls.PowerShell.Provider;

namespace PoshCode.Pansies.Provider
{
    [CmdletProvider("Pansies", ProviderCapabilities.None)]
    public class EntityProvider : CodeOwls.PowerShell.Provider.Provider
    {
        /// <summary>
        /// a required P2F override
        ///
        /// supplies P2F with the path processor for this provider
        /// </summary>
        protected override IPathResolver PathResolver
        {
            get { return null; }
        }

        protected override IPathResolver PathResolver2(string path)
        {
            var name = path.Split([System.IO.Path.DirectorySeparatorChar, System.IO.Path.PathSeparator], StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            var drive = (from driveInfo in ProviderInfo.Drives
                        where StringComparer.OrdinalIgnoreCase.Equals(driveInfo.Root.Trim('\\'), name)
                        select driveInfo).FirstOrDefault() as Drive;

            return drive.PathResolver ?? new PansiesResolver(() => new PansiesProviderRoot());
        }
        /// <summary>
        /// overridden to supply a default drive when the provider is loaded
        /// </summary>
        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            var drives = new Collection<PSDriveInfo>
            {
                new PansiesDrive (
                    new PSDriveInfo( "Fg", ProviderInfo, "ForegroundColors:" + System.IO.Path.DirectorySeparatorChar, "Foreground Colors", null, "Fg:" ),
                    new PansiesResolver(() => new RgbColorProviderRoot(RgbColorMode.Foreground))
                ),
                new PansiesDrive(
                    new PSDriveInfo( "Bg", ProviderInfo, "BackgroundColors:" + System.IO.Path.DirectorySeparatorChar, "Background Colors", null, "Bg:" ),
                    new PansiesResolver(() => new RgbColorProviderRoot(RgbColorMode.Background))
                ),
                new PansiesDrive(
                    new PSDriveInfo( "Esc", ProviderInfo, "EscapeSequences:" + System.IO.Path.DirectorySeparatorChar, "Escape Sequences", null, "Esc:" ),
                    new PansiesResolver(() => Entities.EscapeSequences.ToDriveRoot("Esc"))
                ),
                new PansiesDrive(
                    new PSDriveInfo( "Extra", ProviderInfo, "Strings:" + System.IO.Path.DirectorySeparatorChar, "Named Extended Strings", null, "Extra:" ),
                    new PansiesResolver(() => Entities.ExtendedCharacters.ToDriveRoot("Extra"))
                ),
            };
            if (Entities.EnableNerdFonts) {
                drives.Add(
                    new PansiesDrive(
                        new PSDriveInfo( "NF", ProviderInfo, "NerdFontSymbols:" + System.IO.Path.DirectorySeparatorChar, "NerdFont Symbols", null, "NF:" ),
                        new PansiesResolver(() => Entities.NerdFontSymbols.ToDriveRoot("Extra"))
                    )
                );
            }
            if (Entities.EnableEmoji) {
                drives.Add(
                    new PansiesDrive(
                        new PSDriveInfo( "Emoji", ProviderInfo, "Emoji:" + System.IO.Path.DirectorySeparatorChar, "Emoji 16", null, "Emoji:" ),
                        new PansiesResolver(() => Entities.Emoji.ToDriveRoot("Extra"))
                    )
                );
            };
            return drives;
        }
    }
}
