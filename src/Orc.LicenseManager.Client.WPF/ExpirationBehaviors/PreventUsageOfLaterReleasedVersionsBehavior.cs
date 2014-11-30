// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PreventUsageOfAnyVersionExpirationBehavior.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using Catel.Reflection;

    public class PreventUsageOfLaterReleasedVersionsExpirationBehavior : ExpirationBehaviorBase
    {
        public override bool IsExpired(DateTime expirationDateTime)
        {
            var entryAssembly = AssemblyHelper.GetEntryAssembly();
            var linkerTimestamp = RetrieveLinkerTimestamp(entryAssembly.Location);

            return (linkerTimestamp > expirationDateTime);
        }

        // TODO: Replace by catel
        private DateTime RetrieveLinkerTimestamp(string fileName)
        {
            var buffer = new byte[Math.Max(Marshal.SizeOf(typeof(_IMAGE_FILE_HEADER)), 4)];
            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                fileStream.Position = 0x3C;
                fileStream.Read(buffer, 0, 4);
                fileStream.Position = BitConverter.ToUInt32(buffer, 0); // COFF header offset
                fileStream.Read(buffer, 0, 4); // "PE\0\0"
                fileStream.Read(buffer, 0, buffer.Length);
            }

            var pinnedBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                var coffHeader = (_IMAGE_FILE_HEADER)Marshal.PtrToStructure(pinnedBuffer.AddrOfPinnedObject(), typeof(_IMAGE_FILE_HEADER));
                return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1) + new TimeSpan(coffHeader.TimeDateStamp * TimeSpan.TicksPerSecond));
            }
            finally
            {
                pinnedBuffer.Free();
            }
        }

#pragma warning disable 169
#pragma warning disable 649
        struct _IMAGE_FILE_HEADER
        {
            public ushort Machine;
            public ushort NumberOfSections;
            public uint TimeDateStamp;
            public uint PointerToSymbolTable;
            public uint NumberOfSymbols;
            public ushort SizeOfOptionalHeader;
            public ushort Characteristics;
        };
#pragma warning restore 169
#pragma warning restore 649
    }
}