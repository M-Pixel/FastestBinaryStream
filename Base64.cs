/**
 * Copyright 2019 Max Pixel
 *
 * This file is part of FastestBinaryStream by Max Pixel.
 *
 * FastestBinaryStream is free software: you can redistribute it and/or modify it under the terms of the GNU Lesser
 * General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 *
 * FastestBinaryStream is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the
 * implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License
 * for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License along with FastestBinaryStream.  If not, see
 * <https://www.gnu.org/licenses/>.
 */

using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;

namespace FastestBinaryStream
{
	public unsafe ref partial struct BinaryStream
	{
		private static readonly char[] Base64StringLookup =
		{
			'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
			'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
			'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+',
			'/', '='
		};

		private static readonly byte[] Base64ASCIILookup =
			Encoding.ASCII.GetBytes("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=");
		
		/// <summary>
		/// Converts the stream, from position 0 to the head, to their equivalent <see cref="string"/> representation
		/// as encoded with base-64 digits.
		/// </summary>
		/// <returns>
		/// The <see cref="string"/> representation, in base 64, of the stream from position 0 to the head.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public string FlushBase64String()
		{
			var length = _head - _memory;
			_head = _memory;
			var x = ReadBase64String((int) length);
			return x;
		}

		/// <summary>
		/// Converts the stream, from position 0 to the head, to their equivalent <see cref="string"/> representation
		/// as encoded with base-64 digits.
		/// </summary>
		/// <param name="value">
		/// Returns the <see cref="string"/> representation, in base 64, of the stream from position 0 to the head.
		/// </param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream FlushBase64String(out string value)
		{
			value = FlushBase64String();
			return this;
		}

		/// <summary>
		/// Converts a subset of the stream to its equivalent <see cref="string"/> representation as encoded with
		/// base-64 digits.
		/// </summary>
		/// <param name="lengthBytes">The quantity of bytes to convert.</param>
		/// <param name="resultLengthChars">
		/// Returns the quantity of characters required for the base 64 representation.
		/// </param>
		/// <returns>
		/// The <see cref="string"/> representation, in base 64, of the stream, starting from the head.
		/// </returns>
		[PublicAPI]
		public string ReadBase64String(int lengthBytes, out int resultLengthChars)
		{
			/**
			 * Regarding the contents of this method, which are derived from Mono's MIT-licensed implementation of
			 * System.Convert.ConvertToBase64Array:
			 * 
			 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
			 * associated documentation files (the "Software"), to deal in the Software without restriction, including
			 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
			 * copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
			 * following conditions:
			 *
			 * The above copyright notice and this permission notice shall be included in all copies or substantial
			 * portions of the Software.
			 *
			 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
			 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO
			 * EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
			 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR
			 * THE USE OR OTHER DEALINGS IN THE SOFTWARE.
			 */
			resultLengthChars = (int) (lengthBytes / 3L * 4L + (lengthBytes % 3 != 0 ? 4L : 0L));
			var value = new string('\0', resultLengthChars);
			fixed (char* outChars = value)
			{
				var remainder = lengthBytes % 3;
				var quantityDivisibleBy3 = lengthBytes - remainder;
				var outCharsIndex = 0;
				fixed (char* lookup = &Base64StringLookup[0])
				{
					for (var upperBound = _head + quantityDivisibleBy3; _head < upperBound; _head += 3)
					{
						outChars[outCharsIndex] = lookup[(_head[0] & 252) >> 2];
						outChars[outCharsIndex + 1] = lookup[(_head[0] & 3) << 4 | (_head[1] & 240) >> 4];
						outChars[outCharsIndex + 2] = lookup[(_head[1] & 15) << 2 | (_head[2] & 192) >> 6];
						outChars[outCharsIndex + 3] = lookup[_head[2] & 63];
						outCharsIndex += 4;
					}

					switch (remainder)
					{
						case 2:
							outChars[outCharsIndex] = lookup[(_head[0] & 0xFC) >> 2];
							outChars[outCharsIndex + 1] =
								lookup[(_head[0] & 0x03) << 4 | (_head[1] & 0xF0) >> 4];
							outChars[outCharsIndex + 2] = lookup[(_head[1] & 0x0F) << 2];
							outChars[outCharsIndex + 3] = lookup[64];
							++_head;
							break;

						case 1:
							outChars[outCharsIndex] = lookup[(_head[0] & 0xFC) >> 2];
							outChars[outCharsIndex + 1] = lookup[(_head[0] & 0x03) << 4];
							outChars[outCharsIndex + 2] = lookup[64];
							outChars[outCharsIndex + 3] = lookup[64];
							break;
					}
				}
			}

			return value;
		}

		/// <summary>
		/// Converts a subset of the stream to its equivalent <see cref="string"/> representation as encoded with
		/// base-64 digits.
		/// </summary>
		/// <param name="lengthBytes">The quantity of bytes to convert.</param>
		/// <returns>
		/// The <see cref="string"/> representation, in base 64, of the stream, starting from the head.
		/// </returns>
		[PublicAPI]
		public string ReadBase64String(int lengthBytes) => ReadBase64String(lengthBytes, out int _);

		/// <summary>
		/// Converts a subset of the stream to its equivalent <see cref="string"/> representation as encoded with
		/// base-64 digits.
		/// </summary>
		/// <param name="lengthBytes">The quantity of bytes to convert.</param>
		/// <param name="value">
		/// Returns the <see cref="string"/> representation, in base 64, of the stream, starting from the head.
		/// </param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream ReadBase64String(int lengthBytes, out string value)
		{
			value = ReadBase64String(lengthBytes);
			return this;
		}
		
		/// <summary>
		/// Converts the stream, from position 0 to the head, to their equivalent ASCII string representation as encoded
		/// with base-64 digits.
		/// </summary>
		/// <returns>
		/// The ASCII string representation, in base 64, of the stream from position 0 to the head.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public byte[] FlushBase64ASCII()
		{
			var length = _head - _memory;
			_head = _memory;
			var x = ReadBase64ASCII((int) length);
			return x;
		}

		/// <summary>
		/// Converts the stream, from position 0 to the head, to their equivalent ASCII string representation as encoded
		/// with base-64 digits.
		/// </summary>
		/// <param name="value">
		/// Returns the <see cref="string"/> representation, in base 64, of the stream from position 0 to the head.
		/// </param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream FlushBase64ASCII(out byte[] value)
		{
			value = FlushBase64ASCII();
			return this;
		}

		/// <summary>
		/// Converts a subset of the stream to its equivalent ASCII string representation as encoded with base-64
		/// digits.
		/// </summary>
		/// <param name="lengthBytes">The quantity of bytes to convert.</param>
		/// <param name="resultLengthChars">
		/// Returns the quantity of characters required for the base 64 representation.
		/// </param>
		/// <returns>
		/// The ASCII string representation, in base 64, of the stream, starting from the head.
		/// </returns>
		[PublicAPI]
		public byte[] ReadBase64ASCII(int lengthBytes, out int resultLengthChars)
		{
			/**
			 * Regarding the contents of this method, which are derived from Mono's MIT-licensed implementation of
			 * System.Convert.ConvertToBase64Array:
			 * 
			 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
			 * associated documentation files (the "Software"), to deal in the Software without restriction, including
			 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
			 * copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
			 * following conditions:
			 *
			 * The above copyright notice and this permission notice shall be included in all copies or substantial
			 * portions of the Software.
			 *
			 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
			 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO
			 * EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
			 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR
			 * THE USE OR OTHER DEALINGS IN THE SOFTWARE.
			 */
			resultLengthChars = (int) (lengthBytes / 3L * 4L + (lengthBytes % 3 != 0 ? 4L : 0L));
			var value = new byte[resultLengthChars];
			fixed (byte* outChars = value)
			{
				var remainder = lengthBytes % 3;
				var quantityDivisibleBy3 = lengthBytes - remainder;
				var outCharsIndex = 0;
				fixed (byte* lookup = &Base64ASCIILookup[0])
				{
					for (var upperBound = _head + quantityDivisibleBy3; _head < upperBound; _head += 3)
					{
						outChars[outCharsIndex] = lookup[(_head[0] & 252) >> 2];
						outChars[outCharsIndex + 1] = lookup[(_head[0] & 3) << 4 | (_head[1] & 240) >> 4];
						outChars[outCharsIndex + 2] = lookup[(_head[1] & 15) << 2 | (_head[2] & 192) >> 6];
						outChars[outCharsIndex + 3] = lookup[_head[2] & 63];
						outCharsIndex += 4;
					}

					switch (remainder)
					{
						case 2:
							outChars[outCharsIndex] = lookup[(_head[0] & 0xFC) >> 2];
							outChars[outCharsIndex + 1] =
								lookup[(_head[0] & 0x03) << 4 | (_head[1] & 0xF0) >> 4];
							outChars[outCharsIndex + 2] = lookup[(_head[1] & 0x0F) << 2];
							outChars[outCharsIndex + 3] = lookup[64];
							++_head;
							break;

						case 1:
							outChars[outCharsIndex] = lookup[(_head[0] & 0xFC) >> 2];
							outChars[outCharsIndex + 1] = lookup[(_head[0] & 0x03) << 4];
							outChars[outCharsIndex + 2] = lookup[64];
							outChars[outCharsIndex + 3] = lookup[64];
							break;
					}
				}
			}

			return value;
		}

		/// <summary>
		/// Converts a subset of the stream to its equivalent ASCII string representation as encoded with base-64
		/// digits.
		/// </summary>
		/// <param name="lengthBytes">The quantity of bytes to convert.</param>
		/// <returns>
		/// The ASCII string representation, in base 64, of the stream, starting from the head.
		/// </returns>
		[PublicAPI]
		public byte[] ReadBase64ASCII(int lengthBytes) => ReadBase64ASCII(lengthBytes, out int _);

		/// <summary>
		/// Converts a subset of the stream to its equivalent ASCII string representation as encoded with base-64
		/// digits.
		/// </summary>
		/// <param name="lengthBytes">The quantity of bytes to convert.</param>
		/// <param name="value">
		/// Returns the ASCII string representation, in base 64, of the stream, starting from the head.
		/// </param>
		/// <returns><c>this</c>, for fluent operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining), PublicAPI]
		public BinaryStream ReadBase64ASCII(int lengthBytes, out byte[] value)
		{
			value = ReadBase64ASCII(lengthBytes);
			return this;
		}
	}
}
