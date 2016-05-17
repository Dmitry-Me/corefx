﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Immutable;
using Xunit;

namespace System.Reflection.Metadata.Tests
{
    public class BlobContentIdTests
    {
        [Fact]
        public void Ctor()
        {
            var id1 = new BlobContentId(default(Guid), 1);
            Assert.Equal(default(Guid), id1.Guid);
            Assert.Equal(1u, id1.Stamp);
            Assert.False(id1.IsDefault);

            var id2 = new BlobContentId(new Guid("D6D61CDE-5BAF-4E77-ADDD-3B80F4020BF2"), 0x12345678);
            Assert.Equal(new Guid("D6D61CDE-5BAF-4E77-ADDD-3B80F4020BF2"), id2.Guid);
            Assert.Equal(0x12345678u, id2.Stamp);
            Assert.False(id2.IsDefault);

            Assert.True(default(BlobContentId).IsDefault);
        }

        [Fact]
        public void FromHash()
        {
            var id1 = BlobContentId.FromHash(ImmutableArray.Create<byte>(
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20));

            AssertEx.Equal(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x48, 0x89, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 }, id1.Guid.ToByteArray());
            AssertEx.Equal(new byte[] { 0x11, 0x12, 0x13, 0x94 }, BitConverter.GetBytes(id1.Stamp));

            var id2 = BlobContentId.FromHash(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            AssertEx.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, id2.Guid.ToByteArray());
            AssertEx.Equal(new byte[] { 0x00, 0x00, 0x00, 0x80 }, BitConverter.GetBytes(id2.Stamp));

            var id3 = BlobContentId.FromHash(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff });

            AssertEx.Equal(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x4f, 0xbf, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, id3.Guid.ToByteArray());
            AssertEx.Equal(new byte[] { 0xff, 0xff, 0xff, 0xff }, BitConverter.GetBytes(id3.Stamp));

            Assert.Throws<ArgumentNullException>(() => BlobContentId.FromHash(default(ImmutableArray<byte>)));
            Assert.Throws<ArgumentNullException>(() => BlobContentId.FromHash(null));
            Assert.Throws<ArgumentException>(() => BlobContentId.FromHash(new byte[0]));
            Assert.Throws<ArgumentException>(() => BlobContentId.FromHash(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
        }
    }
}