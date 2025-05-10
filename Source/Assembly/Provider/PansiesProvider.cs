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
    public class PansiesProvider : CodeOwls.PowerShell.Provider.Provider
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
            var name = path.Split([System.IO.Path.DirectorySeparatorChar, ':', System.IO.Path.AltDirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            var drive = (from driveInfo in ProviderInfo.Drives
                         where StringComparer.OrdinalIgnoreCase.Equals(driveInfo.Root.Trim(System.IO.Path.DirectorySeparatorChar, ':', System.IO.Path.AltDirectorySeparatorChar), name)
                         select driveInfo).FirstOrDefault() as Drive;

            return drive.PathResolver ?? new PathNodeResolver(() => new PansiesProviderRoot());
        }
        /// <summary>
        /// overridden to supply a default drive when the provider is loaded
        /// </summary>
        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            var drives = new Collection<PSDriveInfo>
            {
                new PansiesDrive (
                    new PSDriveInfo( "fg", ProviderInfo, "fg:" + System.IO.Path.DirectorySeparatorChar, "Foreground Colors", null, "Fg:" ),
                    new PathNodeResolver(() => new RgbColorProviderRoot(RgbColorMode.Foreground))
                ),
                new PansiesDrive(
                    new PSDriveInfo( "bg", ProviderInfo, "bg:" + System.IO.Path.DirectorySeparatorChar, "Background Colors", null, "Bg:" ),
                    new PathNodeResolver(() => new RgbColorProviderRoot(RgbColorMode.Background))
                ),
                new PansiesDrive(
                    new PSDriveInfo( "esc", ProviderInfo, "esc:" + System.IO.Path.DirectorySeparatorChar, "Escape Sequences", null, "Esc:" ),
                    new PathNodeResolver(() => Entities.EscapeSequences.ToDriveRoot("esc"))
                ),
                new PansiesDrive(
                    new PSDriveInfo( "extra", ProviderInfo, "extra:" + System.IO.Path.DirectorySeparatorChar, "Named Extended Strings", null, "Extra:" ),
                    new PathNodeResolver(() => Entities.ExtendedCharacters.ToDriveRoot("extra"))
                ),
            };
            if (Entities.EnableNerdFonts) {
                drives.Add(
                    new PansiesDrive(
                        new PSDriveInfo("nf", ProviderInfo, "nf:" + System.IO.Path.DirectorySeparatorChar, "NerdFont Symbols", null, "NF:"),
                        new PathNodeResolver(() => Entities.NerdFontSymbols.ToDriveRoot("nf"))
                    )
                );
            }
            if (Entities.EnableEmoji) {
                drives.Add(
                    new PansiesDrive(
                        new PSDriveInfo("emoji", ProviderInfo, "emoji:" + System.IO.Path.DirectorySeparatorChar, "Emoji 16", null, "Emoji:"),
                        new PathNodeResolver(() => Entities.Emoji.ToDriveRoot("emoji"))
                    )
                );
            };
            return drives;
        }
    }
}
