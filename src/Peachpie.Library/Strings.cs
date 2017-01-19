﻿using Pchp.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pchp.Library.Resources;

namespace Pchp.Library
{
    public static class Strings
    {
        #region Character map

        [ThreadStatic]
        private static CharMap _charmap;

        /// <summary>
        /// Get clear <see cref="CharMap"/> to be used by current thread. <see cref="_charmap"/>.
        /// </summary>
        internal static CharMap InitializeCharMap()
        {
            CharMap result = _charmap;

            if (result == null)
                _charmap = result = new CharMap(0x0800);
            else
                result.ClearAll();

            return result;
        }

        #endregion

        #region ord, chr, bin2hex

        /// <summary>
        /// Returns ASCII code of the first character of a string of bytes or <c>0</c> if string is empty.
        /// </summary>
        public static int ord(string str) => string.IsNullOrEmpty(str) ? 0 : (int)str[0];

        /// <summary>
        /// Converts ordinal number of character to a binary string containing that character.
        /// </summary>
        /// <param name="charCode">The ASCII code.</param>
        /// <returns>The character with <paramref name="charCode"/> ASCIT code.</returns>
        public static string chr(int charCode) => unchecked((char)charCode).ToString();

        /// <summary>
        /// Converts ordinal number of Unicode character to a string containing that character.
        /// </summary>
        /// <param name="charCode">The ordinal number of character.</param>
        /// <returns>The character with <paramref name="charCode"/> ordnial number.</returns>
        /*public*/
        static string chr_unicode(int charCode)
        {
            return unchecked((char)charCode).ToString();
        }

        ///// <summary>
        ///// Converts a string of bytes into hexadecimal representation.
        ///// </summary>
        ///// <param name="bytes">The string of bytes.</param>
        ///// <returns>Concatenation of hexadecimal values of bytes of <paramref name="bytes"/>.</returns>
        ///// <example>
        ///// The string "01A" is converted into string "303140" because ord('0') = 0x30, ord('1') = 0x31, ord('A') = 0x40.
        ///// </example>
        //public static string bin2hex(PhpString bytes)
        //{
        //    return (bytes == null) ? String.Empty : StringUtils.BinToHex(bytes.ReadonlyData, null);
        //}

        /// <summary>
        /// Converts a string into hexadecimal representation.
        /// </summary>
        /// <param name="str">The string to be converted.</param>
        /// <returns>
        /// The concatenated four-characters long hexadecimal numbers each representing one character of <paramref name="str"/>.
        /// </returns>
        public static string bin2hex(string str)
        {
            if (str == null) return null;

            int length = str.Length;
            StringBuilder result = new StringBuilder(length * 4, length * 4);
            result.Length = length * 4;

            const string hex_digs = "0123456789abcdef";

            for (int i = 0; i < length; i++)
            {
                int c = (int)str[i];
                result[4 * i + 0] = hex_digs[(c & 0xf000) >> 12];
                result[4 * i + 1] = hex_digs[(c & 0x0f00) >> 8];
                result[4 * i + 2] = hex_digs[(c & 0x00f0) >> 4];
                result[4 * i + 3] = hex_digs[(c & 0x000f)];
            }

            return result.ToString();
        }

        #endregion

        #region strrev, strspn, strcspn

        /// <summary>
        /// Reverses the given string.
        /// </summary>
        /// <param name="str">The string to be reversed.</param>
        /// <returns>The reversed string or empty string if <paramref name="str"/> is null.</returns>
        public static string strrev(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            //
            var chars = str.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        ///// <summary>
        ///// Finds a length of an initial segment consisting entirely of specified characters.
        ///// </summary>
        ///// <param name="str">The string to be searched in.</param>
        ///// <param name="acceptedChars">Accepted characters.</param>
        ///// <returns>
        ///// The length of the initial segment consisting entirely of characters in <paramref name="acceptedChars"/>
        ///// or zero if any argument is null.
        ///// </returns>
        //[ImplementsFunction("strspn")]
        //public static int StrSpn(string str, string acceptedChars)
        //{
        //    return StrSpnInternal(str, acceptedChars, 0, int.MaxValue, false);
        //}

        ///// <summary>
        ///// Finds a length of a segment consisting entirely of specified characters.
        ///// </summary>
        ///// <param name="str">The string to be searched in.</param>
        ///// <param name="acceptedChars">Accepted characters.</param>
        ///// <param name="offset">The relativized offset of the first item of the slice.</param>
        ///// <returns>
        ///// The length of the substring consisting entirely of characters in <paramref name="acceptedChars"/> or 
        ///// zero if any argument is null. Search starts from absolutized <paramref name="offset"/>
        ///// (see <see cref="PhpMath.AbsolutizeRange"/> where <c>length</c> is infinity).
        ///// </returns>
        //[ImplementsFunction("strspn")]
        //public static int StrSpn(string str, string acceptedChars, int offset)
        //{
        //    return StrSpnInternal(str, acceptedChars, offset, int.MaxValue, false);
        //}

        ///// <summary>
        ///// Finds a length of a segment consisting entirely of specified characters.
        ///// </summary>
        ///// <param name="str">The string to be searched in.</param>
        ///// <param name="acceptedChars">Accepted characters.</param>
        ///// <param name="offset">The relativized offset of the first item of the slice.</param>
        ///// <param name="length">The relativized length of the slice.</param>
        ///// <returns>
        ///// The length of the substring consisting entirely of characters in <paramref name="acceptedChars"/> or 
        ///// zero if any argument is null. Search starts from absolutized <paramref name="offset"/>
        ///// (see <see cref="PhpMath.AbsolutizeRange"/> and takes at most absolutized <paramref name="length"/> characters.
        ///// </returns>
        //[ImplementsFunction("strspn")]
        //public static int StrSpn(string str, string acceptedChars, int offset, int length)
        //{
        //    return StrSpnInternal(str, acceptedChars, offset, length, false);
        //}

        ///// <summary>
        ///// Finds a length of an initial segment consisting entirely of any characters excpept for specified ones.
        ///// </summary>
        ///// <param name="str">The string to be searched in.</param>
        ///// <param name="acceptedChars">Accepted characters.</param>
        ///// <returns>
        ///// The length of the initial segment consisting entirely of characters not in <paramref name="acceptedChars"/>
        ///// or zero if any argument is null.
        ///// </returns>
        //[ImplementsFunction("strcspn")]
        //public static int StrCSpn(string str, string acceptedChars)
        //{
        //    return StrSpnInternal(str, acceptedChars, 0, int.MaxValue, true);
        //}

        ///// <summary>
        ///// Finds a length of a segment consisting entirely of any characters excpept for specified ones.
        ///// </summary>
        ///// <param name="str">The string to be searched in.</param>
        ///// <param name="acceptedChars">Accepted characters.</param>
        ///// <param name="offset">The relativized offset of the first item of the slice.</param>
        ///// <returns>
        ///// The length of the substring consisting entirely of characters not in <paramref name="acceptedChars"/> or 
        ///// zero if any argument is null. Search starts from absolutized <paramref name="offset"/>
        ///// (see <see cref="PhpMath.AbsolutizeRange"/> where <c>length</c> is infinity).
        ///// </returns>
        //[ImplementsFunction("strcspn")]
        //public static int StrCSpn(string str, string acceptedChars, int offset)
        //{
        //    return StrSpnInternal(str, acceptedChars, offset, int.MaxValue, true);
        //}

        ///// <summary>
        ///// Finds a length of a segment consisting entirely of any characters except for specified ones.
        ///// </summary>
        ///// <param name="str">The string to be searched in.</param>
        ///// <param name="acceptedChars">Accepted characters.</param>
        ///// <param name="offset">The relativized offset of the first item of the slice.</param>
        ///// <param name="length">The relativized length of the slice.</param>
        ///// <returns>
        ///// The length of the substring consisting entirely of characters not in <paramref name="acceptedChars"/> or 
        ///// zero if any argument is null. Search starts from absolutized <paramref name="offset"/>
        ///// (see <see cref="PhpMath.AbsolutizeRange"/> and takes at most absolutized <paramref name="length"/> characters.
        ///// </returns>
        //[ImplementsFunction("strcspn")]
        //public static int StrCSpn(string str, string acceptedChars, int offset, int length)
        //{
        //    return StrSpnInternal(str, acceptedChars, offset, length, true);
        //}

        ///// <summary>
        ///// Internal version of <see cref="StrSpn"/> (complement off) and <see cref="StrCSpn"/> (complement on).
        ///// </summary>
        //internal static int StrSpnInternal(string str, string acceptedChars, int offset, int length, bool complement)
        //{
        //    if (str == null || acceptedChars == null) return 0;

        //    PhpMath.AbsolutizeRange(ref offset, ref length, str.Length);

        //    char[] chars = acceptedChars.ToCharArray();
        //    Array.Sort(chars);

        //    int j = offset;

        //    if (complement)
        //    {
        //        while (length > 0 && ArrayUtils.BinarySearch(chars, str[j]) < 0) { j++; length--; }
        //    }
        //    else
        //    {
        //        while (length > 0 && ArrayUtils.BinarySearch(chars, str[j]) >= 0) { j++; length--; }
        //    }

        //    return j - offset;
        //}

        #endregion

        #region explode, implode

        /// <summary>
        /// Splits a string by string separators.
        /// </summary>
        /// <param name="separator">The substrings separator. Must not be empty.</param>
        /// <param name="str">The string to be split.</param>
        /// <returns>The array of strings.</returns>
        [return: CastToFalse]
        public static PhpArray explode(string separator, string str) => explode(separator, str, int.MaxValue);

        /// <summary>
        /// Splits a string by string separators with limited resulting array size.
        /// </summary>
        /// <param name="separator">The substrings separator. Must not be empty.</param>
        /// <param name="str">The string to be split.</param>
        /// <param name="limit">
        /// The maximum number of elements in the resultant array. Zero value is treated in the same way as 1.
        /// If negative, then the number of separators found in the string + 1 is added to the limit.
        /// </param>
        /// <returns>The array of strings.</returns>
        /// <remarks>
        /// If <paramref name="str"/> is empty an array consisting of exacty one empty string is returned.
        /// If <paramref name="limit"/> is zero
        /// </remarks>
        /// <exception cref="PhpException">Thrown if the <paramref name="separator"/> is null or empty or if <paramref name="limit"/>is not positive nor -1.</exception>
        [return: CastToFalse]
        public static PhpArray explode(string separator, string str, int limit)
        {
            // validate parameters:
            if (string.IsNullOrEmpty(separator))
            {
                //PhpException.InvalidArgument("separator", LibResources.GetString("arg_null_or_empty"));
                //return null;
                throw new ArgumentException();
            }

            if (str == null) str = String.Empty;

            bool last_part_is_the_rest = limit >= 0;

            if (limit == 0)
                limit = 1;
            else if (limit < 0)
                limit += SubstringCountInternal(str, separator, 0, str.Length) + 2;

            // splits <str> by <separator>:
            int sep_len = separator.Length;
            int i = 0;                        // start searching at this position
            int pos;                          // found separator's first character position
            PhpArray result = new PhpArray(); // creates integer-keyed array with default capacity

            var/*!*/compareInfo = System.Globalization.CultureInfo.InvariantCulture.CompareInfo;

            while (--limit > 0)
            {
                pos = compareInfo.IndexOf(str, separator, i, str.Length - i, System.Globalization.CompareOptions.Ordinal);

                if (pos < 0) break; // not found

                result.AddValue(PhpValue.Create(str.Substring(i, pos - i))); // faster than Add()
                i = pos + sep_len;
            }

            // Adds last chunk. If separator ends the string, it will add empty string (as PHP do).
            if (i <= str.Length && last_part_is_the_rest)
            {
                result.AddValue(PhpValue.Create(str.Substring(i)));
            }

            return result;
        }

        /// <summary>
        /// Concatenates items of an array into a string separating them by a glue.
        /// </summary>
        /// <param name="pieces">The array to be impleded.</param>
        /// <returns>The glued string.</returns>
        public static PhpString join(PhpArray pieces) => implode(pieces);

        /// <summary>
        /// Concatenates items of an array into a string separating them by a glue.
        /// </summary>
        /// <param name="pieces">The array to be impleded.</param>
        /// <param name="glue">The glue string.</param>
        /// <returns>The glued string.</returns>
        /// <exception cref="PhpException">Thrown if neither <paramref name="glue"/> nor <paramref name="pieces"/> is not null and of type <see cref="PhpArray"/>.</exception>
        public static PhpString join(PhpValue glue, PhpValue pieces) => implode(glue, pieces);

        /// <summary>
        /// Concatenates items of an array into a string.
        /// </summary>
        /// <param name="pieces">The <see cref="PhpArray"/> to be imploded.</param>
        /// <returns>The glued string.</returns>
        public static PhpString implode(PhpArray pieces)
        {
            if (pieces == null)
            {
                //PhpException.ArgumentNull("pieces");
                //return null;
                throw new ArgumentException();
            }

            return ImplodeInternal(PhpValue.Void, pieces);
        }

        /// <summary>
        /// Concatenates items of an array into a string separating them by a glue.
        /// </summary>
        /// <param name="glue">The glue of type <see cref="string"/> or <see cref="PhpArray"/> to be imploded.</param>
        /// <param name="pieces">The <see cref="PhpArray"/> to be imploded or glue of type <see cref="string"/>.</param>
        /// <returns>The glued string.</returns>
        /// <exception cref="PhpException">Thrown if neither <paramref name="glue"/> nor <paramref name="pieces"/> is not null and of type <see cref="PhpArray"/>.</exception>
        public static PhpString implode(PhpValue glue, PhpValue pieces)
        {
            if (pieces != null && pieces.IsArray)
                return ImplodeInternal(glue, pieces.AsArray());

            if (glue.IsArray)
                return ImplodeInternal(pieces, glue.AsArray());

            return ImplodeGenericEnumeration(glue, pieces);
        }

        private static PhpString ImplodeGenericEnumeration(PhpValue glue, PhpValue pieces)
        {
            IEnumerable enumerable;

            if (pieces.IsObject && (enumerable = pieces.Object as IEnumerable) != null)
                return ImplodeInternal(glue, new PhpArray(enumerable));

            if (glue.IsObject && (enumerable = glue.Object as IEnumerable) != null)
                return ImplodeInternal(pieces, new PhpArray(enumerable));

            ////
            //PhpException.InvalidArgument("pieces");
            //return null;
            throw new ArgumentException();
        }

        /// <summary>
        /// Concatenates items of an array into a string separating them by a glue.
        /// </summary>
        /// <param name="glue">The glue string.</param>
        /// <param name="pieces">The enumeration to be imploded.</param>
        /// <returns>The glued string.</returns>           
        /// <remarks>
        /// Items of <paramref name="pieces"/> are converted to strings in the manner of PHP 
        /// (i.e. by <see cref="PHP.Core.Convert.ObjectToString"/>).
        /// </remarks>
        /// <exception cref="PhpException">Thrown if <paramref name="pieces"/> is null.</exception>
        private static PhpString ImplodeInternal(PhpValue glue, PhpArray/*!*/pieces)
        {
            Debug.Assert(pieces != null);

            // handle empty pieces:
            if (pieces.Count == 0)
                return PhpString.Empty;

            // check whether we have to preserve a binary string
            //bool binary = glue != null && glue.GetType() == typeof(PhpBytes);
            //if (!binary)    // try to find any binary string within pieces:
            //    using (var x = pieces.GetFastEnumerator())
            //        while (x.MoveNext())
            //            if (x.CurrentValue.IsBinaryString)
            //            {
            //                binary = true;
            //                break;
            //            }

            // concatenate pieces and glue:

            bool not_first = false;                       // not the first iteration

            //if (binary)
            //{
            //    Debug.Assert(pieces.Count > 0);

            //    PhpBytes gluebytes = PHP.Core.Convert.ObjectToPhpBytes(glue);
            //    PhpBytes[] piecesBytes = new PhpBytes[pieces.Count + pieces.Count - 1]; // buffer of PhpBytes to be concatenated
            //    int p = 0;

            //    using (var x = pieces.GetFastEnumerator())
            //        while (x.MoveNext())
            //        {
            //            if (not_first) piecesBytes[p++] = gluebytes;
            //            else not_first = true;

            //            piecesBytes[p++] = PHP.Core.Convert.ObjectToPhpBytes(x.CurrentValue);
            //        }

            //    return PhpBytes.Concat(piecesBytes, 0, piecesBytes.Length);
            //}
            //else
            {
                string gluestr = glue.ToString();

                var result = new PhpString(/*pieces.Count * 2*/);

                using (var x = pieces.GetFastEnumerator())
                    while (x.MoveNext())
                    {
                        if (not_first) result.Append(gluestr);
                        else not_first = true;

                        result.Append(x.CurrentValue.ToString());
                    }

                return result;
            }
        }

        #endregion

        #region strtr, str_rot13

        /// <summary>
        /// Replaces specified characters in a string with another ones.
        /// </summary>
        /// <param name="str">A string where to do the replacement.</param>
        /// <param name="from">Characters to be replaced.</param>
        /// <param name="to">Characters to replace those in <paramref name="from"/> with.</param>
        /// <returns>
        /// A copy of <paramref name="str"/> with all occurrences of each character in <paramref name="from"/> 
        /// replaced by the corresponding character in <paramref name="to"/>.
        /// </returns>
        /// <remarks>
        /// <para>If <paramref name="from"/> and <paramref name="to"/> are different lengths, the extra characters 
        /// in the longer of the two are ignored.</para>
        /// </remarks>
        public static string strtr(string str, string from, string to)
        {
            if (String.IsNullOrEmpty(str) || from == null || to == null) return String.Empty;

            int min_length = Math.Min(from.Length, to.Length);
            Dictionary<char, char> ht = new Dictionary<char, char>(min_length);

            // adds chars to the hashtable:
            for (int i = 0; i < min_length; i++)
                ht[from[i]] = to[i];

            // creates result builder:
            StringBuilder result = new StringBuilder(str.Length, str.Length);
            result.Length = str.Length;

            // translates:
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                char h;
                result[i] = ht.TryGetValue(c, out h) ? h : c;

                // obsolete:
                // object h = ht[c];
                // result[i] = (h==null) ? c : h;
            }

            return result.ToString();
        }

        /// <summary>
        /// Compares objects according to the length of their string representation
        /// as the primary criteria and the alphabetical order as the secondary one.
        /// </summary>
        private sealed class KeyLengthComparer : IComparer<KeyValuePair<string, string>>
        {
            /// <summary>
            /// Performs length and alphabetical comparability backwards (longer first).
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Compare(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
            {
                int rv = x.Key.Length - y.Key.Length;
                if (rv == 0) return -string.CompareOrdinal(x.Key, y.Key);
                else return -rv;
            }
        }

        /// <summary>
        /// Replaces substrings according to a dictionary.
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <param name="replacePairs">
        /// An dictionary that contains <see cref="string"/> to <see cref="string"/> replacement mapping.
        /// </param>
        /// <returns>A copy of str, replacing all substrings (looking for the longest possible match).</returns>
        /// <remarks>This function will not try to replace stuff that it has already worked on.</remarks>
        /// <exception cref="PhpException">Thrown if the <paramref name="replacePairs"/> argument is null.</exception>
        [return: CastToFalse]
        public static string strtr(string str, PhpArray replacePairs)
        {
            if (replacePairs == null)
            {
                //PhpException.ArgumentNull("replacePairs");
                //return null;
                throw new ArgumentException();
            }

            if (string.IsNullOrEmpty(str))
                return String.Empty;

            // sort replacePairs according to the key length, longer first
            var count = replacePairs.Count;
            var sorted = new KeyValuePair<string, string>[count];

            int i = 0;
            var replacePairsEnum = replacePairs.GetFastEnumerator();
            while (replacePairsEnum.MoveNext())
            {
                var key = replacePairsEnum.CurrentKey.ToString();
                var value = replacePairsEnum.CurrentValue.ToString();

                if (key.Length == 0)
                {
                    //// TODO: an exception ?
                    //return null;
                    throw new ArgumentException();
                }

                sorted[i++] = new KeyValuePair<string, string>(key, value);
            }

            Array.Sort<KeyValuePair<string, string>>(sorted, new KeyLengthComparer());

            // perform replacement
            StringBuilder result = new StringBuilder(str);
            StringBuilder temp = new StringBuilder(str);
            int length = str.Length;
            int[] offset = new int[length];

            for (i = 0; i < sorted.Length; i++)
            {
                var key = sorted[i].Key;
                int index = 0;

                while ((index = temp.ToString().IndexOf(key, index, StringComparison.Ordinal)) >= 0)   // ordinal search, because of exotic Unicode characters are find always at the beginning of the temp
                {
                    var value = sorted[i].Value;
                    var keyLength = key.Length;
                    int replaceAtIndex = index + offset[index];

                    // replace occurrence in result
                    result.Remove(replaceAtIndex, keyLength);
                    result.Insert(replaceAtIndex, value);

                    // Pack the offset array (drop the items removed from temp)
                    for (int j = index + keyLength; j < offset.Length; j++)
                        offset[j - keyLength] = offset[j];

                    // Ensure that we don't replace stuff that we already have worked on by
                    // removing the replaced substring from temp.
                    temp.Remove(index, keyLength);
                    for (int j = index; j < length; j++) offset[j] += value.Length;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// GetUserEntryPoint encode a string by shifting every letter (a-z, A-Z) by 13 places in the alphabet.
        /// </summary>
        /// <param name="str">The string to be encoded.</param>
        /// <returns>The string with characters rotated by 13 places.</returns>
        public static string str_rot13(string str)
        {
            return strtr(str,
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                "nopqrstuvwxyzabcdefghijklmNOPQRSTUVWXYZABCDEFGHIJKLM");
        }

        #endregion

        #region substr, str_repeat

        /// <summary>
        /// Retrieves a substring from the given string.
        /// </summary>
        /// <param name="str">The source string (unicode or binary).</param>
        /// <param name="offset">The relativized offset of the first item of the slice.</param>
        /// <param name="length">The relativized length of the slice.</param>
        /// <returns>The substring of the <paramref name="str"/>.</returns>
        /// <remarks>
        /// See <see cref="PhpMath.AbsolutizeRange"/> for details about <paramref name="offset"/> and <paramref name="length"/>.
        /// </remarks>
        [return: CastToFalse]
        public static string substr(string str, int offset, int length = int.MaxValue)
        {
            //PhpBytes binstr = str as PhpBytes;
            //if (binstr != null)
            //{
            //    if (binstr.Length == 0) return null;

            //    PhpMath.AbsolutizeRange(ref offset, ref length, binstr.Length);

            //    // string is shorter than offset to start substring
            //    if (offset == binstr.Length) return null;

            //    if (length == 0) return PhpBytes.Empty;

            //    byte[] substring = new byte[length];

            //    Buffer.BlockCopy(binstr.ReadonlyData, offset, substring, 0, length);

            //    return new PhpBytes(substring);
            //}

            string unistr = str; // Core.Convert.ObjectToString(str);
            if (unistr != null)
            {
                if (unistr == String.Empty) return null;

                PhpMath.AbsolutizeRange(ref offset, ref length, unistr.Length);

                // string is shorter than offset to start substring
                if (offset == unistr.Length) return null;

                if (length == 0) return String.Empty;

                return unistr.Substring(offset, length);
            }

            return null;
        }

        /// <summary>
        /// Repeats a string.
        /// </summary>
        /// <param name="str">The input string, can be both binary and unicode.</param>
        /// <param name="count">The number of times <paramref name="str"/> should be repeated.</param>
        /// <returns>The string where <paramref name="str"/> is repeated <paramref name="count"/> times.</returns>
        /// <remarks>If <paramref name="str"/> is <b>null</b> reference, the function will return an empty string.</remarks>   
        /// <remarks>If <paramref name="count"/> is set to 0, the function will return <b>null</b> reference.</remarks>   
        /// <exception cref="PhpException">Thrown if <paramref name="count"/> is negative.</exception>
        //[PureFunction]
        public static string str_repeat(string str, int count)
        {
            if (str == null) return String.Empty;

            if (count < 0)
            {
                //PhpException.Throw(PhpError.Warning, LibResources.GetString("number_of_repetitions_negative"));
                //return null;
                throw new ArgumentException();
            }
            if (count == 0) return null;

            //PhpBytes binstr = str as PhpBytes;
            //if (binstr != null)
            //{
            //    byte[] result = new byte[binstr.Length * count];

            //    for (int i = 0; i < count; i++) Buffer.BlockCopy(binstr.ReadonlyData, 0, result, binstr.Length * i, binstr.Length);

            //    return new PhpBytes(result);
            //}

            string unistr = str; // Core.Convert.ObjectToString(str);
            if (unistr != null)
            {
                StringBuilder result = new StringBuilder(count * unistr.Length);
                while (count-- > 0) result.Append(unistr);

                return result.ToString();
            }

            return null;
        }

        #endregion

        #region substr_count internals

        private static bool SubstringCountInternalCheck(string needle)
        {
            if (string.IsNullOrEmpty(needle))
            {
                PhpException.InvalidArgument(nameof(needle), Resources.LibResources.arg_null_or_empty);
                return false;
            }

            return true;
        }
        private static bool SubstringCountInternalCheck(string haystack, int offset)
        {
            if (offset < 0)
            {
                PhpException.Throw(PhpError.Warning, LibResources.substr_count_offset_zero);
                return false;
            }
            if (offset > haystack.Length)
            {
                PhpException.Throw(PhpError.Warning, string.Format(LibResources.substr_count_offset_exceeds, offset));
                return false;
            }

            return true;
        }
        private static bool SubstringCountInternalCheck(string haystack, int offset, int length)
        {
            if (!SubstringCountInternalCheck(haystack, offset))
            {
                return false;
            }

            if (length == 0)
            {
                PhpException.Throw(PhpError.Warning, LibResources.substr_count_zero_length);
                return false;
            }
            if (offset + length > haystack.Length)
            {
                PhpException.Throw(PhpError.Warning, string.Format(LibResources.substr_count_length_exceeds, length));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Count the number of substring occurrences. Expects correct argument values.
        /// </summary>
        internal static int SubstringCountInternal(string/*!*/ haystack, string/*!*/ needle, int offset, int end)
        {
            int result = 0;

            if (needle.Length == 1)
            {
                while (offset < end)
                {
                    if (haystack[offset] == needle[0]) result++;
                    offset++;
                }
            }
            else
            {
                while ((offset = haystack.IndexOf(needle, offset, end - offset)) != -1)
                {
                    offset += needle.Length;
                    result++;
                }
            }
            return result;
        }

        #endregion

        #region substr_count, substr_replace, substr_compare

        /// <summary>
        /// See <see cref="substr_count(string,string,int,int)"/>.
        /// </summary>
        [return: CastToFalse]
        public static int substr_count(string haystack, string needle, int offset = 0)
        {
            if (String.IsNullOrEmpty(haystack)) return 0;
            if (!SubstringCountInternalCheck(needle)) return -1;
            if (!SubstringCountInternalCheck(haystack, offset)) return -1;

            return SubstringCountInternal(haystack, needle, offset, haystack.Length);
        }

        /// <summary>
        /// Count the number of substring occurrences.
        /// </summary>
        /// <param name="haystack">The string.</param>
        /// <param name="needle">The substring.</param>
        /// <param name="offset">The relativized offset of the first item of the slice. Zero if missing in overloads</param>
        /// <param name="length">The relativized length of the slice. Infinity if missing in overloads.</param>
        /// <returns>The number of <paramref name="needle"/> occurences in <paramref name="haystack"/>.</returns>
        /// <example>"aba" has one occurence in "ababa".</example>
        /// <remarks>
        /// See <see cref="PhpMath.AbsolutizeRange"/> for details about <paramref name="offset"/> and <paramref name="length"/>.
        /// </remarks>
        /// <exception cref="PhpException">Thrown if <paramref name="needle"/> is null.</exception>
        [return: CastToFalse]
        public static int substr_count(string haystack, string needle, int offset, int length)
        {
            if (string.IsNullOrEmpty(haystack)) return 0;
            if (!SubstringCountInternalCheck(needle)) return -1;
            if (!SubstringCountInternalCheck(haystack, offset, length)) return -1;

            return SubstringCountInternal(haystack, needle, offset, offset + length);
        }

        /// <summary>
        /// See <see cref="substr_replace(PhpValue, PhpValue, PhpValue, PhpValue)"/>.
        /// </summary>
        public static string substr_replace(string subject, string replacement, int offset)
        {
            return SubstringReplace(subject, replacement, offset, int.MaxValue);
        }

        /// <summary>
        /// Replaces a portion of a string or multiple strings with another string.
        /// </summary>
        /// <param name="ctx">Current context. Cannot be <c>null</c>.</param>
        /// <param name="subject">The subject of replacement (can be an array of subjects).</param>
        /// <param name="replacement">The replacement string (can be array of replacements).</param>
        /// <param name="offset">The relativized offset of the first item of the slice (can be array of offsets).</param>
        /// <param name="length">The relativized length of the slice (can be array of lengths).</param>
        /// <returns>
        /// Either the <paramref name="subject"/> with a substring replaced by <paramref name="replacement"/> if it is a string
        /// or an array containing items of the <paramref name="subject"/> with substrings replaced by <paramref name="replacement"/>
        /// and indexed by integer keys starting from 0. If <paramref name="replacement"/> is an array, multiple replacements take place.
        /// </returns>
        /// <remarks>
        /// See <see cref="PhpMath.AbsolutizeRange"/> for details about <paramref name="offset"/> and <paramref name="length"/>.
        /// Missing <paramref name="length"/> is considered to be infinity.
        /// If <paramref name="offset"/> and <paramref name="length"/> conversion results in position
        /// less than or equal to zero and greater than or equal to string length, the replacement is prepended and appended, respectively.
        /// </remarks>
        public static PhpValue substr_replace(Context ctx, PhpValue subject, PhpValue replacement, PhpValue offset, PhpValue length)
        {
            IList<PhpValue> subject_list, replacement_list, offset_list, length_list;
            string[] replacements = null, subjects = null;
            int[] offsets = null, lengths = null;
            int int_offset = 0, int_length = 0;
            string str_replacement = null;

            // prepares string array of subjects:
            if ((subject_list = subject.Object as IList<PhpValue>) != null)
            {
                subjects = new string[subject_list.Count];
                int i = 0;
                foreach (var item in subject_list)
                {
                    subjects[i++] = item.ToString(ctx);
                }
            }
            else
            {
                subjects = new string[] { subject.ToString(ctx) };
            }

            // prepares string array of replacements:
            if ((replacement_list = replacement.Object as IList<PhpValue>) != null)
            {
                replacements = new string[replacement_list.Count];
                int i = 0;
                foreach (var item in replacement_list)
                {
                    replacements[i++] = item.ToString(ctx);
                }
            }
            else
            {
                str_replacement = replacement.ToString(ctx);
            }

            // prepares integer array of offsets:
            if ((offset_list = offset.Object as IList<PhpValue>) != null)
            {
                offsets = new int[offset_list.Count];
                int i = 0;
                foreach (var item in offset_list)
                {
                    offsets[i++] = (int)item.ToLong();
                }
            }
            else
            {
                int_offset = (int)offset.ToLong();
            }

            // prepares integer array of lengths:
            if ((length_list = length.Object as IList<PhpValue>) != null)
            {
                lengths = new int[length_list.Count];
                int i = 0;
                foreach (var item in length_list)
                {
                    lengths[i++] = (int)item.ToLong();
                }
            }
            else
            {
                int_length = (int)length.ToLong();
            }

            for (int i = 0; i < subjects.Length; i++)
            {
                if (offset_list != null) int_offset = (i < offsets.Length) ? offsets[i] : 0;
                if (length_list != null) int_length = (i < lengths.Length) ? lengths[i] : subjects[i].Length;
                if (replacement_list != null) str_replacement = (i < replacements.Length) ? replacements[i] : string.Empty;

                subjects[i] = SubstringReplace(subjects[i], str_replacement, int_offset, int_length);
            }

            if (subject_list != null)
                return PhpValue.Create(new PhpArray(subjects));
            else
                return PhpValue.Create(subjects[0]);
        }

        /// <summary>
        /// Performs substring replacements on subject.
        /// </summary>
        static string SubstringReplace(string subject, string replacement, int offset, int length)
        {
            PhpMath.AbsolutizeRange(ref offset, ref length, subject.Length);
            return new StringBuilder(subject).Remove(offset, length).Insert(offset, replacement).ToString();
        }

        /// <summary>
        /// Compares substrings.
        /// </summary>
        /// <param name="mainStr">A string whose substring to compare with <paramref name="str"/>.</param>
        /// <param name="str">The second operand of the comparison.</param>
        /// <param name="offset">An offset in <paramref name="mainStr"/> where to start. Negative value means zero. Offsets beyond <paramref name="mainStr"/> means its length.</param>
        /// <param name="length">A maximal number of characters to compare. Non-positive values means infinity.</param>
        /// <param name="ignoreCase">Whether to ignore case.</param>
        public static int substr_compare(string mainStr, string str, int offset, int length = Int32.MaxValue, bool ignoreCase = false)
        {
            if (mainStr == null) mainStr = string.Empty;
            if (str == null) str = string.Empty;
            if (length <= 0) length = int.MaxValue;
            if (offset < 0) offset = 0;
            if (offset > mainStr.Length) offset = mainStr.Length;

            return string.Compare(mainStr, offset, str, 0, length, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        #endregion

        #region str_replace, str_ireplace

        static PhpValue str_replace(Context ctx, PhpValue search, PhpValue replace, PhpValue subject, ref long count, StringComparison compareType)
        {
            var subjectArr = subject.AsArray();
            if (subjectArr == null)
            {
                // string
                return PhpValue.Create(str_replace(ctx, search, replace, subject.ToStringOrThrow(ctx), ref count, compareType));
            }
            else
            {
                // array
                return PhpValue.Create(str_replace(ctx, search, replace, subjectArr, ref count, compareType));
            }
        }

        static PhpArray str_replace(Context ctx, PhpValue search, PhpValue replace, PhpArray subject, ref long count, StringComparison compareType)
        {
            var result = new PhpArray(subject.Count);
            var enumerator = subject.GetFastEnumerator();
            while (enumerator.MoveNext())
            {
                result.AddValue(PhpValue.Create(str_replace(ctx, search, replace, enumerator.CurrentValue.ToStringOrThrow(ctx), ref count, compareType)));
            }

            return result;
        }

        static string str_replace(Context ctx, PhpValue search, PhpValue replace, string subject, ref long count, StringComparison compareType)
        {
            if (string.IsNullOrEmpty(subject))
            {
                return string.Empty;
            }

            //
            var searchArr = search.AsArray();
            if (searchArr == null)
            {
                // string -> string
                subject = str_replace(search.ToStringOrThrow(ctx), replace.ToStringOrThrow(ctx), subject, ref count, compareType);
            }
            else
            {
                var searchEnum = searchArr.GetFastEnumerator();

                var replaceArr = replace.AsArray();
                if (replaceArr != null)
                {
                    // array -> array
                    var replaceEnum = replaceArr.GetFastEnumerator();
                    while (searchEnum.MoveNext())
                    {
                        var searchStr = searchEnum.CurrentValue.ToStringOrThrow(ctx);
                        var replaceStr = replaceEnum.MoveNext() ? replaceEnum.CurrentValue.ToStringOrThrow(ctx) : string.Empty;
                        subject = str_replace(searchStr, replaceStr, subject, ref count, compareType);
                    }
                }
                else
                {
                    // array -> string
                    var replaceStr = replace.ToStringOrThrow(ctx);
                    while (searchEnum.MoveNext())
                    {
                        var searchStr = searchEnum.CurrentValue.ToStringOrThrow(ctx);
                        subject = str_replace(searchStr, replaceStr, subject, ref count, compareType);
                    }
                }
            }

            //
            return subject;
        }

        static string str_replace(string search, string replace, string subject, ref long count, StringComparison compareType)
        {
            if (string.IsNullOrEmpty(search))
            {
                return subject;
            }

            //if (replace == null)
            //{
            //    replace = string.Empty;
            //}
            Debug.Assert(replace != null);

            // temporary result instantiated lazily
            StringBuilder result = null;

            // elementary replace
            int index, from = 0;
            while ((index = subject.IndexOf(search, from, compareType)) >= 0)
            {
                if (result == null)
                {
                    result = new StringBuilder(subject.Length);
                }

                result.Append(subject, from, index - from);
                result.Append(replace);
                from = index + search.Length;
                count++;
            }

            if (result == null)
            {
                return subject;
            }
            else
            {
                result.Append(subject, from, subject.Length - from);
                return result.ToString();
            }
        }

        /// <summary>
        /// Replaces all occurrences of the <paramref name="search"/> string 
        /// with the <paramref name="replace"/> string.
        /// </summary>
        public static PhpValue str_replace(Context ctx, PhpValue search, PhpValue replace, PhpValue subject)
        {
            long count = 0;
            return str_replace(ctx, search, replace, subject, ref count, StringComparison.Ordinal);
        }

        /// <summary>
        /// Replaces all occurrences of the <paramref name="search"/> string 
        /// with the <paramref name="replace"/> string counting the number of occurrences.
        /// </summary>
        /// <param name="ctx">Current context. Cannot be <c>null</c>.</param>
        /// <param name="search">
        /// The substring(s) to replace. Can be <see cref="string"/> or <see cref="PhpArray"/> of strings.
        /// </param>
        /// <param name="replace">
        /// The string(s) to replace <paramref name="search"/>. Can be <see cref="string"/> or <see cref="PhpArray"/> containing strings.
        /// </param>
        /// <param name="subject">
        /// The <see cref="string"/> or <see cref="PhpArray"/> of strings to perform the search and replace with.
        /// </param>
        /// <param name="count">
        /// The number of matched and replaced occurrences.
        /// </param>
        /// <returns>
        /// A <see cref="string"/> or <see cref="PhpArray"/> with
        /// all occurrences of <paramref name="search"/> in <paramref name="subject"/>
        /// replaced with the given <paramref name="replace"/> value.
        /// </returns>
        public static PhpValue str_replace(Context ctx, PhpValue search, PhpValue replace, PhpValue subject, out long count)
        {
            count = 0;
            return str_replace(ctx, search, replace, subject, ref count, StringComparison.Ordinal);
        }

        public static PhpValue str_ireplace(Context ctx, PhpValue search, PhpValue replace, PhpValue subject)
        {
            long count = 0;
            return str_replace(ctx, search, replace, subject, ref count, StringComparison.OrdinalIgnoreCase);
        }

        public static PhpValue str_ireplace(Context ctx, PhpValue search, PhpValue replace, PhpValue subject, out long count)
        {
            count = 0;
            return str_replace(ctx, search, replace, subject, ref count, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region str_shuffle, str_split

        /// <summary>
        /// Randomly shuffles a string.
        /// </summary>
        /// <param name="str">The string to shuffle.</param>
        /// <returns>One random permutation of <paramref name="str"/>.</returns>
        public static string str_shuffle(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            int count = str.Length;
            if (count <= 1)
            {
                return str;
            }

            var generator = PhpMath.Generator;
            var newstr = new StringBuilder(str);

            // Takes n-th character from the string at random with probability 1/i
            // and exchanges it with the one on the i-th position.
            // Thus a random permutation is formed in the second part of the string (from i to count)
            // and the set of remaining characters is stored in the first part.
            for (int i = count - 1; i > 0; i--)
            {
                int n = generator.Next(i + 1);
                char ch = newstr[i];
                newstr[i] = newstr[n];
                newstr[n] = ch;
            }

            //
            return newstr.ToString();
        }

        /// <summary>
        /// Converts a string to an array.
        /// </summary>
        /// <param name="str">The string to split.</param>
        /// <returns>An array with keys being character indeces and values being characters.</returns>
        [return: CastToFalse]
        public static PhpArray str_split(string str)
        {
            return Split(str, 1);
        }

        /// <summary>
        /// Converts a string to an array.
        /// </summary>
        /// <param name="ctx">Current context. Cannot be <c>null</c>.</param>
        /// <param name="obj">The string to split.</param>
        /// <param name="splitLength">Length of chunks <paramref name="obj"/> should be split into.</param>
        /// <returns>An array with keys being chunk indeces and values being chunks of <paramref name="splitLength"/>
        /// length.</returns>
        /// <exception cref="PhpException">The <paramref name="splitLength"/> parameter is not positive (Warning).</exception>
        [return: CastToFalse]
        public static PhpArray str_split(Context ctx, PhpValue obj, int splitLength)
        {
            if (splitLength < 1)
            {
                throw new ArgumentOutOfRangeException();
                //PhpException.Throw(PhpError.Warning, LibResources.GetString("segment_length_not_positive"));
                //return null;
            }
            if (obj == null)
            {
                return new PhpArray();
            }

            var phpstr = obj.Object as PhpString;
            if (phpstr != null && phpstr.ContainsBinaryData)
            {
                return Split(phpstr.ToBytes(ctx.StringEncoding), splitLength);
            }
            else
            {
                return Split(obj.ToString(ctx), splitLength);
            }
        }

        static PhpArray Split(string str, int splitLength)
        {
            int length = str.Length;
            PhpArray result = new PhpArray(length / splitLength + 1);

            // add items of length splitLength
            int i;
            for (i = 0; i < (length - splitLength + 1); i += splitLength)
            {
                result.Add(str.Substring(i, splitLength));
            }

            // add the last item
            if (i < length) result.Add(str.Substring(i));

            return result;
        }

        static PhpArray Split(byte[] str, int splitLength)
        {
            int length = str.Length;
            PhpArray result = new PhpArray(length / splitLength + 1);

            // add items of length splitLength
            int i;
            for (i = 0; i < (length - splitLength + 1); i += splitLength)
            {
                byte[] chunk = new byte[splitLength];
                Array.Copy(str, i, chunk, 0, chunk.Length);
                result.Add(PhpValue.Create(new PhpString(chunk)));
            }

            // add the last item
            if (i < length)
            {
                byte[] chunk = new byte[length - i];
                Array.Copy(str, i, chunk, 0, chunk.Length);
                result.Add(PhpValue.Create(new PhpString(chunk)));
            }

            return result;
        }

        #endregion

        #region quoted_printable_decode, quoted_printable_encode

        /// <summary>
        /// Maximum length of line according to quoted-printable specification.
        /// </summary>
        internal const int PHP_QPRINT_MAXL = 75;

        /// <summary>
        /// Converts a quoted-printable string into (an 8-bit) string.
        /// </summary>
        /// <param name="ctx">Runtime context.</param>
        /// <param name="str">The quoted-printable string.</param>
        /// <returns>The 8-bit string corresponding to the decoded <paramref name="str"/>.</returns>
        /// <remarks>Based on the implementation in quot_print.c PHP source file.</remarks>
        public static string quoted_printable_decode(Context ctx, string str)
        {
            if (str == null)
            {
                return string.Empty;
            }

            Encoding encoding = ctx.StringEncoding;
            MemoryStream stream = new MemoryStream();
            StringBuilder result = new StringBuilder(str.Length / 2);

            int i = 0;
            while (i < str.Length)
            {
                char c = str[i];

                if (c == '=')
                {
                    if (i + 2 < str.Length && UriUtils.IsHexDigit(str[i + 1]) && UriUtils.IsHexDigit(str[i + 2]))
                    {
                        stream.WriteByte((byte)((UriUtils.FromHex(str[i + 1]) << 4) + UriUtils.FromHex(str[i + 2])));
                        i += 3;
                    }
                    else  // check for soft line break according to RFC 2045
                    {
                        int k = 1;

                        // Possibly, skip spaces/tabs at the end of line
                        while (i + k < str.Length && (str[i + k] == ' ' || str[i + k] == '\t')) k++;

                        // End of line reached
                        if (i + k >= str.Length)
                        {
                            i += k;
                        }
                        else if (str[i + k] == '\r' && i + k + 1 < str.Length && str[i + k + 1] == '\n')
                        {
                            // CRLF
                            i += k + 2;
                        }
                        else if (str[i + k] == '\r' || str[i + k] == '\n')
                        {
                            // CR or LF
                            i += k + 1;
                        }
                        else
                        {
                            // flush stream
                            if (stream.Position > 0)
                            {
                                result.Append(encoding.GetChars(stream.GetBuffer(), 0, (int)stream.Position));
                                stream.Seek(0, SeekOrigin.Begin);
                            }
                            result.Append(str[i++]);
                        }
                    }
                }
                else
                {
                    // flush stream
                    if (stream.Position > 0)
                    {
                        result.Append(encoding.GetChars(stream.GetBuffer(), 0, (int)stream.Position));
                        stream.Seek(0, SeekOrigin.Begin);
                    }

                    result.Append(c);
                    i++;
                }
            }

            // flush stream
            if (stream.Position > 0)
            {
                result.Append(encoding.GetChars(stream.GetBuffer(), 0, (int)stream.Position));
                stream.Seek(0, SeekOrigin.Begin);
            }

            return result.ToString();
        }

        /// <summary>
        /// Convert a 8 bit string to a quoted-printable string
        /// </summary>
        /// <param name="ctx">Runtime context.</param>
        /// <param name="str">The input string.</param>
        /// <returns>The quoted-printable string.</returns>
        /// <remarks>Based on the implementation in quot_print.c PHP source file.</remarks>
        public static string quoted_printable_encode(Context ctx, string str)
        {
            if (str == null)
            {
                return string.Empty;
            }

            Encoding encoding = ctx.StringEncoding;
            MemoryStream stream = new MemoryStream();

            StringBuilder result = new StringBuilder(3 * str.Length + 3 * (((3 * str.Length) / PHP_QPRINT_MAXL) + 1));
            string hex = "0123456789ABCDEF";

            byte[] bytes = new byte[encoding.GetMaxByteCount(1)];
            int encodedChars;


            int i = 0;
            int j = 0;
            int charsOnLine = 0;
            char c;
            while (i < str.Length)
            {
                c = str[i];

                if (c == '\r' && i + 1 < str.Length && str[i + 1] == '\n')
                {
                    result.Append("\r\n");
                    charsOnLine = 0;
                    i += 2;
                }
                else
                {

                    if (char.IsControl(c) ||
                        c >= 0x7F || // is not ascii char
                        (c == '=') ||
                        ((c == ' ') && i + 1 < str.Length && (str[i + 1] == '\r')))
                    {

                        if ((charsOnLine += 3) > PHP_QPRINT_MAXL)
                        {
                            result.Append("=\r\n");
                            charsOnLine = 3;
                        }

                        // encode c(==str[i])
                        encodedChars = encoding.GetBytes(str, i, 1, bytes, 0);

                        for (j = 0; j < encodedChars; ++j)
                        {
                            result.Append('=');
                            result.Append(hex[bytes[j] >> 4]);
                            result.Append(hex[bytes[j] & 0xf]);
                        }
                    }
                    else
                    {

                        if ((++charsOnLine) > PHP_QPRINT_MAXL)
                        {
                            result.Append("=\r\n");
                            charsOnLine = 1;
                        }
                        result.Append(c);
                    }

                    ++i;
                }
            }
            return result.ToString();
        }

        #endregion

        #region addslashes, addcslashes, quotemeta

        /// <summary>
        /// Adds backslashes before characters depending on current configuration.
        /// </summary>
        /// <param name="str">Data to process.</param>
        /// <returns>
        /// The string or string of bytes where some characters are preceded with the backslash character.
        /// </returns>
        /// <remarks>
        /// If <see cref="LocalConfiguration.VariablesSection.QuoteInDbManner"/> ("magic_quotes_sybase" in PHP) option 
        /// is set then '\0' characters are slashed and single quotes are replaced with two single quotes. Otherwise,
        /// '\'', '"', '\\' and '\0 characters are slashed.
        /// </remarks>
        public static string addslashes(string str) => StringUtils.AddCSlashes(str, true, true);

        /// <summary>
        /// Quote string with slashes in a C style.
        /// </summary>
        public static string addcslashes(string str, string mask)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            if (string.IsNullOrEmpty(mask)) return str;

            //Encoding encoding = Configuration.Application.Globalization.PageEncoding;

            //// to guarantee the same result both the string and the mask has to be converted to bytes:
            //string c = ArrayUtils.ToString(encoding.GetBytes(mask));
            //string s = ArrayUtils.ToString(encoding.GetBytes(str));

            string c = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(mask));
            string s = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(str));

            // the result contains ASCII characters only, so there is no need to conversions:
            return AddCSlashesInternal(str, s, c);
        }

        /// <param name="translatedStr">A sequence of chars or ints from which to take character codes.</param>
        /// <param name="translatedMask">A mask containing codes.</param>
        /// <param name="str">A string to be slashed.</param>
        /// <exception cref="PhpException"><paramref name="translatedStr"/> interval is invalid.</exception>
        /// <exception cref="PhpException"><paramref name="translatedStr"/> contains Unicode characters greater than '\u0800'.</exception>
        static string AddCSlashesInternal(string str, string translatedStr, string translatedMask)
        {
            Debug.Assert(str != null && translatedMask != null && translatedStr != null && str.Length == translatedStr.Length);

            // prepares the mask:
            CharMap charmap = InitializeCharMap();
            try
            {
                charmap.AddUsingMask(translatedMask);
            }
            catch (IndexOutOfRangeException)
            {
                throw; // TODO: Err
                //PhpException.Throw(PhpError.Warning, LibResources.GetString("too_big_unicode_character"));
                //return null;
            }

            const string cslashed_chars = "abtnvfr";

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                //char c = translatedStr[i];

                if (charmap.Contains(translatedStr[i]))
                {
                    result.Append('\\');

                    char c = str[i];    // J: translatedStr and translatedMask are used only in context of CharMap, later we are working with original str only

                    // performs conversion to C representation:
                    if (c < '\u0020' || c > '\u007f')
                    {
                        if (c >= '\u0007' && c <= '\u000d')
                            result.Append(cslashed_chars[c - '\u0007']);
                        else
                            result.Append(System.Convert.ToString((int)c, 8));  // 0x01234567
                    }
                    else
                        result.Append(c);
                }
                else
                    result.Append(str[i]);
            }

            return result.ToString();
        }

        /// <summary>
        /// A map of following characters: {'.', '\', '+', '*', '?', '[', '^', ']', '(', '$', ')'}.
        /// </summary>
        static readonly CharMap metaCharactersMap = new CharMap(new uint[] { 0, 0x08f20001, 0x0000001e });

        /// <summary>
        /// Adds backslashes before following characters: {'.', '\', '+', '*', '?', '[', '^', ']', '(', '$', ')'}
        /// </summary>
        /// <param name="str">The string to be processed.</param>
        /// <returns>The string where said characters are backslashed.</returns>
        public static string quotemeta(string str)
        {
            if (str == null)
            {
                return string.Empty;
            }

            int length = str.Length;
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                char c = str[i];
                if (metaCharactersMap.Contains(c)) result.Append('\\');
                result.Append(c);
            }

            return result.ToString();
        }

        #endregion

        #region stripslashes, stripcslashes

        /// <summary>
        /// Unquote string quoted with <see cref="AddSlashes"/>.
        /// </summary>
        /// <param name="str">The string to unquote.</param>
        /// <returns>The unquoted string.</returns>
        public static string stripslashes(string str)
        {
            return StringUtils.StripCSlashes(str);
        }

        /// <summary>
        /// Returns a string with backslashes stripped off. Recognizes \a, \b, \f, \n, \r, \t, \v, \\, octal
        /// and hexadecimal representation.
        /// </summary>
        /// <param name="ctx">Runtime context.</param>
        /// <param name="str">The string to strip.</param>
        /// <returns>The stripped string.</returns>
        public static string stripcslashes(Context ctx, string str)
        {
            if (str == null)
            {
                return string.Empty;
            }

            Encoding encoding = ctx.StringEncoding;
            const char escape = '\\';
            int length = str.Length;
            StringBuilder result = new StringBuilder(length);
            bool state1 = false;
            byte[] bA1 = new byte[1];

            for (int i = 0; i < length; i++)
            {
                char c = str[i];
                if (c == escape && state1 == false)
                {
                    state1 = true;
                    continue;
                }

                if (state1 == true)
                {
                    switch (c)
                    {
                        case 'a': result.Append('\a'); break;
                        case 'b': result.Append('\b'); break;
                        case 'f': result.Append('\f'); break;
                        case 'n': result.Append('\n'); break;
                        case 'r': result.Append('\r'); break;
                        case 't': result.Append('\t'); break;
                        case 'v': result.Append('\v'); break;
                        case '\\': result.Append('\\'); break;

                        // hex ASCII code
                        case 'x':
                            {
                                int code = 0;
                                if (i + 1 < length && UriUtils.IsHexDigit(str[i + 1])) // first digit
                                {
                                    code = UriUtils.FromHex(str[i + 1]);
                                    i++;
                                    if (i + 1 < length && UriUtils.IsHexDigit(str[i + 1])) // second digit
                                    {
                                        code = (code << 4) + UriUtils.FromHex(str[i + 1]);
                                        i++;
                                    }

                                    bA1[0] = (byte)code;
                                    result.Append(encoding.GetChars(bA1)[0]);
                                    break;
                                }
                                goto default;
                            }

                        // octal ASCII code
                        default:
                            {
                                int code = 0, j = 0;
                                for (; j < 3 && i < length && str[i] >= '0' && str[i] <= '8'; i++, j++)
                                {
                                    code = (code << 3) + (str[i] - '0');
                                }

                                if (j > 0)
                                {
                                    i--;
                                    bA1[0] = (byte)code;
                                    result.Append(encoding.GetChars(bA1)[0]);
                                }
                                else result.Append(c);
                                break;
                            }
                    }

                    state1 = false;
                }
                else result.Append(c);
            }

            return result.ToString();
        }

        #endregion

        #region htmlspecialchars, htmlspecialchars_decode, htmlentities, get_html_translation_table, html_entity_decode

        /// <summary>Quote conversion options.</summary>
        [Flags, PhpHidden]
        public enum QuoteStyle
        {
            /// <summary>
            /// Default quote style for <c>htmlentities</c>.
            /// </summary>
            HtmlEntitiesDefault = QuoteStyle.Compatible | QuoteStyle.Html401,

            /// <summary>Single quotes.</summary>
            SingleQuotes = 1,

            /// <summary>Double quotes.</summary>
            DoubleQuotes = 2,

            /// <summary>
            /// No quotes.
            /// Will leave both double and single quotes unconverted.
            /// </summary>
            NoQuotes = 0,

            /// <summary>
            /// Will convert double-quotes and leave single-quotes alone.
            /// </summary>
            Compatible = DoubleQuotes,

            /// <summary>
            /// Both single and double quotes.
            /// Will convert both double and single quotes.
            /// </summary>
            BothQuotes = DoubleQuotes | SingleQuotes,

            /// <summary>
            /// Silently discard invalid code unit sequences instead of
            /// returning an empty string. Using this flag is discouraged
            /// as it may have security implications.
            /// </summary>
            Ignore = 4,

            /// <summary>
            /// Replace invalid code unit sequences with a Unicode
            /// Replacement Character U+FFFD (UTF-8) or &amp;#FFFD;
            /// (otherwise) instead of returning an empty string.
            /// </summary>
            Substitute = 8,

            /// <summary>
            /// Handle code as HTML 4.01.
            /// </summary>
            Html401 = NoQuotes,

            /// <summary>
            /// Handle code as XML 1.
            /// </summary>
            XML1 = 16,

            /// <summary>
            /// Handle code as XHTML.
            /// </summary>
            XHTML = 32,

            /// <summary>
            /// Handle code as HTML 5.
            /// </summary>
            HTML5 = XML1 | XHTML,

            /// <summary>
            /// Replace invalid code points for the given document type
            /// with a Unicode Replacement Character U+FFFD (UTF-8) or &amp;#FFFD;
            /// (otherwise) instead of leaving them as is.
            /// This may be useful, for instance, to ensure the well-formedness
            /// of XML documents with embedded external content.
            /// </summary>
            Disallowed = 128,
        };

        public const int ENT_NOQUOTES = (int)QuoteStyle.NoQuotes;
        public const int ENT_COMPAT = (int)QuoteStyle.Compatible;
        public const int ENT_QUOTES = (int)QuoteStyle.BothQuotes;
        public const int ENT_IGNORE = (int)QuoteStyle.Ignore;
        public const int ENT_SUBSTITUTE = (int)QuoteStyle.Substitute;
        public const int ENT_HTML401 = (int)QuoteStyle.Html401;
        public const int ENT_XML1 = (int)QuoteStyle.XML1;
        public const int ENT_XHTML = (int)QuoteStyle.XHTML;
        public const int ENT_HTML5 = (int)QuoteStyle.HTML5;
        public const int ENT_DISALLOWED = (int)QuoteStyle.Disallowed;

        /// <summary>Types of HTML entities tables.</summary>
        [PhpHidden]
        public enum HtmlEntitiesTable
        {
            /// <summary>Table containing special characters only.</summary>
            SpecialChars = 0,

            /// <summary>Table containing all entities.</summary>
            AllEntities = 1
        };

        public const int HTML_SPECIALCHARS = (int)HtmlEntitiesTable.SpecialChars;
        public const int HTML_ENTITIES = (int)HtmlEntitiesTable.AllEntities;

        /// <summary>
        /// Converts special characters to HTML entities.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="quoteStyle">Quote conversion.</param>
        /// <param name="charSet">The character set used in conversion. This parameter is ignored.</param>
        /// <param name="doubleEncode">When double_encode is turned off PHP will not encode existing html entities, the default is to convert everything.</param>
        /// <returns>The converted string.</returns>
        public static string htmlspecialchars(string str, QuoteStyle quoteStyle = QuoteStyle.Compatible, string charSet = "ISO-8859-1", bool doubleEncode = true)
        {
            if (!doubleEncode)
            {
                 PhpException.ArgumentValueNotSupported(nameof(doubleEncode), doubleEncode);
            }

            return HtmlSpecialCharsEncode(str, 0, str.Length, quoteStyle, charSet);
        }

        /// <summary>
        /// Converts special characters of substring to HTML entities.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="index">First character of the string to covert.</param>
        /// <param name="length">Length of the substring to covert.</param>
        /// <param name="quoteStyle">Quote conversion.</param>
        /// <param name="charSet">The character set used in conversion. This parameter is ignored.</param>
        /// <returns>The converted substring.</returns>
        static string HtmlSpecialCharsEncode(string str, int index, int length, QuoteStyle quoteStyle, string charSet)
        {
            if (str == null) return String.Empty;

            Debug.Assert(index + length <= str.Length);

            StringBuilder result = new StringBuilder(length);

            // quote style is anded to emulate PHP behavior (any value is allowed):
            string single_quote = (quoteStyle & QuoteStyle.SingleQuotes) != 0 ? "&#039;" : "'";
            string double_quote = (quoteStyle & QuoteStyle.DoubleQuotes) != 0 ? "&quot;" : "\"";

            for (int i = index; i < index + length; i++)
            {
                char c = str[i];
                switch (c)
                {
                    case '&': result.Append("&amp;"); break;
                    case '"': result.Append(double_quote); break;
                    case '\'': result.Append(single_quote); break;
                    case '<': result.Append("&lt;"); break;
                    case '>': result.Append("&gt;"); break;
                    default: result.Append(c); break;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Converts HTML entities (&amp;amp;, &amp;lt;, &amp;gt;, and optionally &amp;quot; and &amp;#039;) 
        /// in a specified string to the respective characters. 
        /// </summary>
        /// <param name="str">The string to be converted.</param>
        /// <param name="quoteStyle">Which quote entities to convert.</param>
        /// <returns>String with converted entities.</returns>
        public static string htmlspecialchars_decode(string str, QuoteStyle quoteStyle = QuoteStyle.Compatible)
        {
            if (str == null) return null;

            StringBuilder result = new StringBuilder(str.Length);

            bool dq = (quoteStyle & QuoteStyle.DoubleQuotes) != 0;
            bool sq = (quoteStyle & QuoteStyle.SingleQuotes) != 0;

            int i = 0;
            while (i < str.Length)
            {
                char c = str[i];
                if (c == '&')
                {
                    i++;
                    if (i + 4 < str.Length && str[i + 4] == ';')                   // quot; #039;
                    {
                        if (dq && str[i] == 'q' && str[i + 1] == 'u' && str[i + 2] == 'o' && str[i + 3] == 't') { i += 5; result.Append('"'); continue; }
                        if (sq && str[i] == '#' && str[i + 1] == '0' && str[i + 2] == '3' && str[i + 3] == '9') { i += 5; result.Append('\''); continue; }
                    }

                    if (i + 3 < str.Length && str[i + 3] == ';')                   // amp; #39;
                    {
                        if (str[i] == 'a' && str[i + 1] == 'm' && str[i + 2] == 'p') { i += 4; result.Append('&'); continue; }
                        if (sq && str[i] == '#' && str[i + 1] == '3' && str[i + 2] == '9') { i += 4; result.Append('\''); continue; }
                    }

                    if (i + 2 < str.Length && str[i + 2] == ';' && str[i + 1] == 't')  // lt; gt;
                    {
                        if (str[i] == 'l') { i += 3; result.Append('<'); continue; }
                        if (str[i] == 'g') { i += 3; result.Append('>'); continue; }
                    }
                }
                else
                {
                    i++;
                }

                result.Append(c);
            }

            return result.ToString();
        }

        /// <summary>
        /// Default <c>encoding</c> used in <c>htmlentities</c>.
        /// </summary>
        const string DefaultHtmlEntitiesCharset = "UTF-8";

        /// <summary>
        /// Converts special characters to HTML entities.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="quoteStyle">Quote conversion.</param>
        /// <param name="charSet">The character set used in conversion. This parameter is ignored.</param>
        /// <param name="doubleEncode">When it is turned off existing HTML entities will not be encoded. The default is to convert everything.</param>
        /// <returns>The converted string.</returns>
        /// <remarks>This method is identical to <see cref="HtmlSpecialChars"/> in all ways, except with
        /// <b>htmlentities</b> (<see cref="EncodeHtmlEntities"/>), all characters that have HTML character entity equivalents are
        /// translated into these entities.</remarks>
        public static string htmlentities(PhpString str, QuoteStyle quoteStyle = QuoteStyle.HtmlEntitiesDefault, string charSet = DefaultHtmlEntitiesCharset, bool doubleEncode = true)
        {
            try
            {
                return EncodeHtmlEntities(str.ToString(charSet), quoteStyle, doubleEncode);
            }
            catch (ArgumentException ex)
            {
                PhpException.Throw(PhpError.Warning, ex.Message);
                return string.Empty;
            }
        }

        static string EncodeHtmlEntities(string str, QuoteStyle quoteStyle, bool doubleEncode)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            if (!doubleEncode)
            {   // existing HTML entities will not be double encoded // TODO: do it nicely
                str = DecodeHtmlEntities(str, quoteStyle);
            }

            // if only double quotes should be encoded, we can use HttpUtility.HtmlEncode right away:
            if ((quoteStyle & QuoteStyle.BothQuotes) == QuoteStyle.DoubleQuotes)
            {
                return System.Net.WebUtility.HtmlEncode(str);
            }

            // quote style is anded to emulate PHP behavior (any value is allowed):
            string single_quote = (quoteStyle & QuoteStyle.SingleQuotes) != 0 ? "&#039;" : "'";
            string double_quote = (quoteStyle & QuoteStyle.DoubleQuotes) != 0 ? "&quot;" : "\"";

            StringBuilder str_builder = new StringBuilder(str.Length);
            StringWriter result = new StringWriter(str_builder);

            // convert ' and " manually, rely on HttpUtility.HtmlEncode for everything else
            char[] quotes = new char[] { '\'', '\"' };
            int old_index = 0, index = 0;
            while (index < str.Length && (index = str.IndexOfAny(quotes, index)) >= 0)
            {
                result.Write(System.Net.WebUtility.HtmlEncode(str.Substring(old_index, index - old_index)));

                if (str[index] == '\'') result.Write(single_quote);
                else result.Write(double_quote);

                old_index = ++index;
            }
            if (old_index < str.Length) result.Write(System.Net.WebUtility.HtmlEncode(str.Substring(old_index)));

            result.Flush();
            return str_builder.ToString();
        }

        /// <summary>
        /// Returns the translation table used by <see cref="HtmlSpecialChars"/> and <see cref="EncodeHtmlEntities"/>. 
        /// </summary>
        /// <param name="table">Type of the table that should be returned.</param>
        /// <param name="quoteStyle">Quote conversion.</param>
        /// <returns>The table.</returns>
        public static PhpArray get_html_translation_table(HtmlEntitiesTable table, QuoteStyle quoteStyle = QuoteStyle.Compatible)
        {
            PhpArray result = new PhpArray();
            if (table == HtmlEntitiesTable.SpecialChars)
            {
                // return the table used with HtmlSpecialChars
                if ((quoteStyle & QuoteStyle.SingleQuotes) != 0) result.Add("\'", "&#039;");
                if ((quoteStyle & QuoteStyle.DoubleQuotes) != 0) result.Add("\"", "&quot;");

                result.Add("&", "&amp;");
                result.Add("<", "&lt;");
                result.Add(">", "&gt;");
            }
            else
            {
                // return the table used with HtmlEntities
                if ((quoteStyle & QuoteStyle.SingleQuotes) != 0) result.Add("\'", "&#039;");
                if ((quoteStyle & QuoteStyle.DoubleQuotes) != 0) result.Add("\"", "&quot;");

                for (char ch = (char)0; ch < 0x100; ch++)
                {
                    if (ch != '\'' && ch != '\"')
                    {
                        string str = ch.ToString();
                        string enc = System.Net.WebUtility.HtmlEncode(str);

                        // if the character was encoded:
                        if (str != enc) result.Add(str, enc);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Converts all HTML entities to their applicable characters.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="quoteStyle">Quote conversion.</param>
        /// <param name="charSet">The character set used in conversion.</param>
        /// <returns>The converted string.</returns>
        public static string html_entity_decode(PhpString str, QuoteStyle quoteStyle = QuoteStyle.Compatible, string charSet = DefaultHtmlEntitiesCharset)
        {
            try
            {
                return DecodeHtmlEntities(str.ToString(charSet), quoteStyle);
            }
            catch (ArgumentException ex)
            {
                PhpException.Throw(PhpError.Warning, ex.Message);
                return string.Empty;
            }
        }

        static string DecodeHtmlEntities(string str, QuoteStyle quoteStyle)
        {
            if (str == null) return String.Empty;

            // if both quotes should be decoded, we can use HttpUtility.HtmlDecode right away:
            if ((quoteStyle & QuoteStyle.BothQuotes) == QuoteStyle.BothQuotes)
            {
                return System.Net.WebUtility.HtmlDecode(str);
            }

            StringBuilder str_builder = new StringBuilder(str.Length);
            StringWriter result = new StringWriter(str_builder);

            // convert &#039;, &#39; and &quot; manually, rely on HttpUtility.HtmlDecode for everything else
            int old_index = 0, index = 0;
            while (index < str.Length && (index = str.IndexOf('&', index)) >= 0)
            {
                // &quot;
                if ((quoteStyle & QuoteStyle.DoubleQuotes) == 0 && index < str.Length - 5 &&
                    str[index + 1] == 'q' && str[index + 2] == 'u' &&
                    str[index + 3] == 'o' && str[index + 4] == 't' &&
                    str[index + 5] == ';')
                {
                    result.Write(System.Net.WebUtility.HtmlDecode(str.Substring(old_index, index - old_index)));
                    result.Write("&quot;");
                    old_index = (index += 6);
                    continue;
                }

                if ((quoteStyle & QuoteStyle.SingleQuotes) == 0)
                {
                    // &#039;
                    if (index < str.Length - 5 && str[index + 1] == '#' &&
                        str[index + 2] == '0' && str[index + 3] == '3' &&
                        str[index + 4] == '9' && str[index + 5] == ';')
                    {
                        result.Write(System.Net.WebUtility.HtmlDecode(str.Substring(old_index, index - old_index)));
                        result.Write("&#039;");
                        old_index = (index += 6);
                        continue;
                    }

                    // &#39;
                    if (index < str.Length - 4 && str[index + 1] == '#' &&
                        str[index + 2] == '3' && str[index + 3] == '9' && str[index + 4] == ';')
                    {
                        result.Write(System.Net.WebUtility.HtmlDecode(str.Substring(old_index, index - old_index)));
                        result.Write("&#39;");
                        old_index = (index += 5);
                        continue;
                    }
                }

                index++; // for the &
            }
            if (old_index < str.Length) result.Write(System.Net.WebUtility.HtmlDecode(str.Substring(old_index)));

            result.Flush();
            return str_builder.ToString();
        }

        #endregion

        #region strip_tags, nl2br

        /// <summary>
        /// Strips HTML and PHP tags from a string.
        /// </summary>
        /// <param name="str">The string to strip tags from.</param>
        /// <returns>The result.</returns>
        public static string strip_tags(string str) => strip_tags(str, null);

        /// <summary>
        /// Strips HTML and PHP tags from a string.
        /// </summary>
        /// <param name="str">The string to strip tags from.</param>
        /// <param name="allowableTags">Tags which should not be stripped in the following format:
        /// &lt;tag1&gt;&lt;tag2&gt;&lt;tag3&gt;.</param>
        /// <returns>The result.</returns>
        /// <remarks>This is a slightly modified php_strip_tags which can be found in PHP sources.</remarks>
        public static string strip_tags(string str, string allowableTags)
        {
            int state = 0;
            return StripTags(str, allowableTags, ref state);
        }

        /// <summary>
        /// Strips tags allowing to set automaton start state and read its accepting state.
        /// </summary>
        internal static string StripTags(string str, string allowableTags, ref int state)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            int br = 0, i = 0, depth = 0, length = str.Length;
            char lc = '\0';

            // Simple state machine. State 0 is the output state, State 1 means we are inside a
            // normal html tag and state 2 means we are inside a php tag.
            //
            // lc holds the last significant character read and br is a bracket counter.
            // When an allowableTags string is passed in we keep track of the string in
            // state 1 and when the tag is closed check it against the allowableTags string
            // to see if we should allow it.

            StringBuilder result = new StringBuilder(), tagBuf = new StringBuilder();
            if (allowableTags != null) allowableTags = allowableTags.ToLower();

            while (i < length)
            {
                char c = str[i];

                switch (c)
                {
                    case '<':
                        if (i + 1 < length && Char.IsWhiteSpace(str[i + 1])) goto default;
                        if (state == 0)
                        {
                            lc = '<';
                            state = 1;
                            if (allowableTags != null)
                            {
                                tagBuf.Length = 0;
                                tagBuf.Append(c);
                            }
                        }
                        else if (state == 1) depth++;
                        break;

                    case '(':
                        if (state == 2)
                        {
                            if (lc != '"' && lc != '\'')
                            {
                                lc = '(';
                                br++;
                            }
                        }
                        else if (allowableTags != null && state == 1) tagBuf.Append(c);
                        else if (state == 0) result.Append(c);
                        break;

                    case ')':
                        if (state == 2)
                        {
                            if (lc != '"' && lc != '\'')
                            {
                                lc = ')';
                                br--;
                            }
                        }
                        else if (allowableTags != null && state == 1) tagBuf.Append(c);
                        else if (state == 0) result.Append(c);
                        break;

                    case '>':
                        if (depth > 0)
                        {
                            depth--;
                            break;
                        }

                        switch (state)
                        {
                            case 1: /* HTML/XML */
                                lc = '>';
                                state = 0;
                                if (allowableTags != null)
                                {
                                    // find out whether this tag is allowable or not
                                    tagBuf.Append(c);

                                    StringBuilder normalized = new StringBuilder();

                                    bool done = false;
                                    int tagBufLen = tagBuf.Length, substate = 0;

                                    // normalize the tagBuf by removing leading and trailing whitespace and turn
                                    // any <a whatever...> into just <a> and any </tag> into <tag>
                                    for (int j = 0; j < tagBufLen; j++)
                                    {
                                        char d = Char.ToLower(tagBuf[j]);
                                        switch (d)
                                        {
                                            case '<':
                                                normalized.Append(d);
                                                break;

                                            case '>':
                                                done = true;
                                                break;

                                            default:
                                                if (!Char.IsWhiteSpace(d))
                                                {
                                                    if (substate == 0)
                                                    {
                                                        substate = 1;
                                                        if (d != '/') normalized.Append(d);
                                                    }
                                                    else normalized.Append(d);
                                                }
                                                else if (substate == 1) done = true;
                                                break;
                                        }
                                        if (done) break;
                                    }

                                    normalized.Append('>');
                                    if (allowableTags.IndexOf(normalized.ToString()) >= 0) result.Append(tagBuf);

                                    tagBuf.Length = 0;
                                }
                                break;

                            case 2: /* PHP */
                                if (br == 0 && lc != '\"' && i > 0 && str[i] == '?') state = 0;
                                {
                                    state = 0;
                                    tagBuf.Length = 0;
                                }
                                break;

                            case 3:
                                state = 0;
                                tagBuf.Length = 0;
                                break;

                            case 4: /* JavaScript/CSS/etc... */
                                if (i >= 2 && str[i - 1] == '-' && str[i - 2] == '-')
                                {
                                    state = 0;
                                    tagBuf.Length = 0;
                                }
                                break;

                            default:
                                result.Append(c);
                                break;
                        }
                        break;

                    case '"':
                        goto case '\'';

                    case '\'':
                        if (state == 2 && i > 0 && str[i - 1] != '\\')
                        {
                            if (lc == c) lc = '\0';
                            else if (lc != '\\') lc = c;
                        }
                        else if (state == 0) result.Append(c);
                        else if (allowableTags != null && state == 1) tagBuf.Append(c);
                        break;

                    case '!':
                        /* JavaScript & Other HTML scripting languages */
                        if (state == 1 && i > 0 && str[i - 1] == '<')
                        {
                            state = 3;
                            lc = c;
                        }
                        else
                        {
                            if (state == 0) result.Append(c);
                            else if (allowableTags != null && state == 1) tagBuf.Append(c);
                        }
                        break;

                    case '-':
                        if (state == 3 && i >= 2 && str[i - 1] == '-' && str[i - 2] == '!') state = 4;
                        else goto default;
                        break;

                    case '?':
                        if (state == 1 && i > 0 && str[i - 1] == '<')
                        {
                            br = 0;
                            state = 2;
                            break;
                        }
                        goto case 'e';

                    case 'E':
                        goto case 'e';

                    case 'e':
                        /* !DOCTYPE exception */
                        if (state == 3 && i > 6
                            && Char.ToLower(str[i - 1]) == 'p' && Char.ToLower(str[i - 2]) == 'y'
                            && Char.ToLower(str[i - 3]) == 't' && Char.ToLower(str[i - 4]) == 'c'
                            && Char.ToLower(str[i - 5]) == 'o' && Char.ToLower(str[i - 6]) == 'd')
                        {
                            state = 1;
                            break;
                        }
                        goto case 'l';

                    case 'l':

                        /*
                          If we encounter '<?xml' then we shouldn't be in
                          state == 2 (PHP). Switch back to HTML.
                        */

                        if (state == 2 && i > 2 && str[i - 1] == 'm' && str[i - 2] == 'x')
                        {
                            state = 1;
                            break;
                        }
                        goto default;

                    /* fall-through */
                    default:
                        if (state == 0) result.Append(c);
                        else if (allowableTags != null && state == 1) tagBuf.Append(c);
                        break;
                }
                i++;
            }

            return result.ToString();
        }

        /// <summary>
        /// Inserts HTML line breaks before all newlines in a string.
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <returns>The output string.</returns>
        /// <remarks>Inserts "&lt;br/&gt;" before each "\n", "\n\r", "\r", "\r\n".</remarks>
        public static string nl2br(string str) => nl2br(str, true);

        /// <summary>
        /// Inserts HTML line breaks before all newlines in a string.
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <param name="isXHTML">Whenever to use XHTML compatible line breaks or not. </param>
        /// <returns>The output string.</returns>
        /// <remarks>Inserts "&lt;br/&gt;" before each "\n", "\n\r", "\r", "\r\n".</remarks>
        public static string nl2br(string str, bool isXHTML/*=true*/ )
        {
            if (string.IsNullOrEmpty(str))
                return String.Empty;

            StringReader reader = new StringReader(str);
            StringWriter writer = new StringWriter(new StringBuilder(str.Length));

            NewLinesToBreaks(reader, writer, isXHTML ? "<br />" : "<br>");

            return writer.ToString();
        }

        internal static void NewLinesToBreaks(TextReader/*!*/ input, TextWriter/*!*/ output, string lineBreakString)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            if (output == null)
                throw new ArgumentNullException("output");

            for (;;)
            {
                int d = input.Read();
                if (d == -1) break;

                char c = (char)d;
                if (c == '\r' || c == '\n')
                {
                    output.Write(lineBreakString);

                    d = input.Peek();
                    if (d != -1)
                    {
                        char c1 = (char)d;
                        if ((c == '\r' && c1 == '\n') || (c == '\n' && c1 == '\r'))
                        {
                            output.Write(c);
                            c = c1;
                            input.Read();
                        }
                    }
                }

                output.Write(c);
            }
        }

        #endregion

        #region chunk_split

        /// <summary>
        /// Splits a string into chunks of a specified length separated by a specified string.
        /// </summary>
        /// <param name="str">The string to split.</param>
        /// <param name="chunkLength">The chunk length.</param>
        /// <param name="endOfChunk">The chunk separator.</param>
        /// <returns><paramref name="endOfChunk"/> is also appended after the last chunk.</returns>
        [return: CastToFalse]
        public static string chunk_split(string str, int chunkLength = 76, string endOfChunk = "\r\n")
        {
            if (str == null) return string.Empty;

            if (chunkLength <= 0)
            {
                //PhpException.InvalidArgument("chunkLength", LibResources.GetString("arg_negative_or_zero"));
                //return null;
                throw new ArgumentException();
            }

            int length = str.Length;
            StringBuilder result = new StringBuilder(length + (length / chunkLength + 1) * endOfChunk.Length);

            // append the chunks one by one to the result
            for (int i = 0, j = length - chunkLength; i < length; i += chunkLength)
            {
                if (i > j) result.Append(str, i, length - i); else result.Append(str, i, chunkLength);
                result.Append(endOfChunk);
            }

            return result.ToString();
        }

        #endregion

        #region soundex, metaphone, levenshtein, similar_text

        /// <summary>
        /// A map of following characters: {'A', 'E', 'I', 'Y', 'O', 'U', 'a', 'e', 'i', 'y', 'o', 'u'}.
        /// </summary>
        internal static readonly CharMap vowelsMap = new CharMap(new uint[] { 0, 0, 0x44410440, 0x44410440 });

        /// <summary>
        /// Indicates whether a character is recognized as an English vowel.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <returns>True iff recognized as an English vowel.</returns>
        internal static bool IsVowel(char c)
        {
            return vowelsMap.Contains(c);
        }

        /// <summary>
        /// Calculates the soundex key of a string.
        /// </summary>
        /// <param name="str">The string to calculate soundex key of.</param>
        /// <returns>The soundex key of <paramref name="str"/>.</returns>
        public static string soundex(string str)
        {
            if (str == null || str == String.Empty) return String.Empty;

            int length = str.Length;
            const string sound = "01230120022455012623010202";

            char[] result = new char[4];
            int resPos = 0;
            char lastIdx = '0';

            for (int i = 0; i < length; i++)
            {
                char c = Char.ToUpper(str[i]);
                if (c >= 'A' && c <= 'Z')
                {
                    char idx = sound[(int)(c - 'A')];
                    if (resPos == 0)
                    {
                        result[resPos++] = c;
                        lastIdx = idx;
                    }
                    else
                    {
                        if (idx != '0' && idx != lastIdx)
                        {
                            result[resPos] = idx;
                            if (++resPos >= 4) return new string(result);
                        }

                        // Some soundex algorithm descriptions say that the following condition should
                        // be in effect...
                        /*if (c != 'W' && c != 'H')*/
                        lastIdx = idx;
                    }
                }
            }

            // pad with '0'
            do
            {
                result[resPos] = '0';
            }
            while (++resPos < 4);

            return new string(result);
        }

        /// <summary>
        /// Calculates the metaphone key of a string.
        /// </summary>
        /// <param name="str">The string to calculate metaphone key of.</param>
        /// <returns>The metaphone key of <paramref name="str"/>.</returns>
        public static string metaphone(string str)
        {
            if (str == null) return String.Empty;

            int length = str.Length;
            const int padL = 4, padR = 3;

            StringBuilder sb = new StringBuilder(str.Length + padL + padR);
            StringBuilder result = new StringBuilder();

            // avoid index out of bounds problem when looking at previous and following characters
            // by padding the string at both sides
            sb.Append('\0', padL);
            sb.Append(str.ToUpper());
            sb.Append('\0', padR);

            int i = padL;
            char c = sb[i];

            // transformations at the beginning of the string
            if ((c == 'A' && sb[i + 1] == 'E') ||
                (sb[i + 1] == 'N' && (c == 'G' || c == 'K' || c == 'P')) ||
                (c == 'W' && sb[i + 1] == 'R')) i++;

            if (c == 'X') sb[i] = 'S';

            if (c == 'W' && sb[i + 1] == 'H') sb[++i] = 'W';

            // if the string starts with a vowel it is copied to output
            if (IsVowel(sb[i])) result.Append(sb[i++]);

            int end = length + padL;
            while (i < end)
            {
                c = sb[i];

                if (c == sb[i - 1] && c != 'C')
                {
                    i++;
                    continue;
                }

                // transformations of consonants (vowels as well as other characters are ignored)
                switch (c)
                {
                    case 'B':
                        if (sb[i - 1] != 'M') result.Append('B');
                        break;

                    case 'C':
                        if (sb[i + 1] == 'I' || sb[i + 1] == 'E' || sb[i + 1] == 'Y')
                        {
                            if (sb[i + 2] == 'A' && sb[i + 1] == 'I') result.Append('X');
                            else if (sb[i - 1] == 'S') break;
                            else result.Append('S');
                        }
                        else if (sb[i + 1] == 'H')
                        {
                            result.Append('X');
                            i++;
                        }
                        else result.Append('K');
                        break;

                    case 'D':
                        if (sb[i + 1] == 'G' && (sb[i + 2] == 'E' || sb[i + 2] == 'Y' ||
                            sb[i + 2] == 'I'))
                        {
                            result.Append('J');
                            i++;
                        }
                        else result.Append('T');
                        break;

                    case 'F':
                        result.Append('F');
                        break;

                    case 'G':
                        if (sb[i + 1] == 'H')
                        {
                            if (sb[i - 4] == 'H' || (sb[i - 3] != 'B' && sb[i - 3] != 'D' && sb[i - 3] != 'H'))
                            {
                                result.Append('F');
                                i++;
                            }
                            else break;
                        }
                        else if (sb[i + 1] == 'N')
                        {
                            if (sb[i + 2] < 'A' || sb[i + 2] > 'Z' ||
                                (sb[i + 2] == 'E' && sb[i + 3] == 'D')) break;
                            else result.Append('K');
                        }
                        else if ((sb[i + 1] == 'E' || sb[i + 1] == 'I' || sb[i + 1] == 'Y') && sb[i - 1] != 'G')
                        {
                            result.Append('J');
                        }
                        else result.Append('K');
                        break;

                    case 'H':
                        if (IsVowel(sb[i + 1]) && sb[i - 1] != 'C' && sb[i - 1] != 'G' &&
                            sb[i - 1] != 'P' && sb[i - 1] != 'S' && sb[i - 1] != 'T') result.Append('H');
                        break;

                    case 'J':
                        result.Append('J');
                        break;

                    case 'K':
                        if (sb[i - 1] != 'C') result.Append('K');
                        break;

                    case 'L':
                        result.Append('L');
                        break;

                    case 'M':
                        result.Append('M');
                        break;

                    case 'N':
                        result.Append('N');
                        break;

                    case 'P':
                        if (sb[i + 1] == 'H') result.Append('F');
                        else result.Append('P');
                        break;

                    case 'Q':
                        result.Append('K');
                        break;

                    case 'R':
                        result.Append('R');
                        break;

                    case 'S':
                        if (sb[i + 1] == 'I' && (sb[i + 2] == 'O' || sb[i + 2] == 'A')) result.Append('X');
                        else if (sb[i + 1] == 'H')
                        {
                            result.Append('X');
                            i++;
                        }
                        else result.Append('S');
                        break;

                    case 'T':
                        if (sb[i + 1] == 'I' && (sb[i + 2] == 'O' || sb[i + 2] == 'A')) result.Append('X');
                        else if (sb[i + 1] == 'H')
                        {
                            result.Append('0');
                            i++;
                        }
                        else result.Append('T');
                        break;

                    case 'V':
                        result.Append('F');
                        break;

                    case 'W':
                        if (IsVowel(sb[i + 1])) result.Append('W');
                        break;

                    case 'X':
                        result.Append("KS");
                        break;

                    case 'Y':
                        if (IsVowel(sb[i + 1])) result.Append('Y');
                        break;

                    case 'Z':
                        result.Append('S');
                        break;
                }

                i++;
            }

            return result.ToString();
        }

        /// <summary>
        /// Calculates the Levenshtein distance between two strings given the cost of insert, replace
        /// and delete operations.
        /// </summary>
        /// <param name="src">The first string.</param>
        /// <param name="dst">The second string.</param>
        /// <param name="insertCost">Cost of the insert operation.</param>
        /// <param name="replaceCost">Cost of the replace operation.</param>
        /// <param name="deleteCost">Cost of the delete operation.</param>
        /// <returns>The Levenshtein distance between <paramref name="src"/> and <paramref name="dst"/> or -1 if any of the
        /// strings is longer than 255 characters.</returns>
        /// <remarks>See <A href="http://www.merriampark.com/ld.htm">http://www.merriampark.com/ld.htm</A> for description of the algorithm.</remarks>
        public static int levenshtein(string src, string dst, int insertCost = 1, int replaceCost = 1, int deleteCost = 1)
        {
            if (src == null) src = String.Empty;
            if (dst == null) dst = String.Empty;

            int n = src.Length;
            int m = dst.Length;

            if (n > 255 || m > 255) return -1;

            if (n == 0) return m * insertCost;
            if (m == 0) return n * deleteCost;

            int[,] matrix = new int[n + 1, m + 1];

            for (int i = 0; i <= n; i++) matrix[i, 0] = i * deleteCost;
            for (int j = 0; j <= m; j++) matrix[0, j] = j * insertCost;

            for (int i = 1; i <= n; i++)
            {
                char cs = src[i - 1];

                for (int j = 1; j <= m; j++)
                {
                    char cd = dst[j - 1];

                    matrix[i, j] = System.Math.Min(System.Math.Min(
                        matrix[i - 1, j] + deleteCost,
                        matrix[i, j - 1] + insertCost),
                        matrix[i - 1, j - 1] + (cs == cd ? 0 : replaceCost));
                }
            }

            return matrix[n, m];
        }

        /// <summary>
        /// Calculates the similarity between two strings. Internal recursive function.
        /// </summary>
        /// <param name="first">The first string.</param>
        /// <param name="second">The second string.</param>
        /// <returns>The number of matching characters in both strings.</returns>
        /// <remarks>Algorithm description is supposed to be found 
        /// <A href="http://citeseer.nj.nec.com/oliver93decision.html">here</A>.</remarks>
        internal static int SimilarTextInternal(string first, string second)
        {
            Debug.Assert(first != null && second != null);

            int posF = 0, lengthF = first.Length;
            int posS = 0, lengthS = second.Length;
            int maxK = 0;

            for (int i = 0; i < lengthF; i++)
            {
                for (int j = 0; j < lengthS; j++)
                {
                    int k;
                    for (k = 0; i + k < lengthF && j + k < lengthS && first[i + k] == second[j + k]; k++) ;
                    if (k > maxK)
                    {
                        maxK = k;
                        posF = i;
                        posS = j;
                    }
                }
            }

            int sum = maxK;
            if (sum > 0)
            {
                if (posF > 0 && posS > 0)
                {
                    sum += SimilarTextInternal(first.Substring(0, posF), second.Substring(0, posS));
                }
                if (posF + maxK < lengthF && posS + maxK < lengthS)
                {
                    sum += SimilarTextInternal(first.Substring(posF + maxK), second.Substring(posS + maxK));
                }
            }

            return sum;
        }

        /// <summary>
        /// Calculates the similarity between two strings.
        /// </summary>
        /// <param name="first">The first string.</param>
        /// <param name="second">The second string.</param>
        /// <returns>The number of matching characters in both strings.</returns>
        public static int similar_text(string first, string second)
        {
            if (first == null || second == null) return 0;
            return SimilarTextInternal(first, second);
        }

        /// <summary>
        /// Calculates the similarity between two strings.
        /// </summary>
        /// <param name="first">The first string.</param>
        /// <param name="second">The second string.</param>
        /// <param name="percent">Will become the similarity in percent.</param>
        /// <returns>The number of matching characters in both strings.</returns>
        public static int similar_text(string first, string second, out double percent)
        {
            if (first == null || second == null) { percent = 0; return 0; }

            int sum = SimilarTextInternal(first, second);
            percent = (200.0 * sum) / (first.Length + second.Length);

            return sum;
        }

        #endregion

        #region strtok

        /// <summary>
        /// Holds a context of <see cref="strtok(Context, string)"/> method.
        /// </summary>
        private sealed class TokenizerContext
        {
            /// <summary>
            /// The <b>str</b> parameter of last <see cref="Tokenize"/> method call.
            /// </summary>
            public string String;

            /// <summary>
            /// Current position in <see cref="TokenizerContext"/>.
            /// </summary>
            public int Position;

            /// <summary>
            /// The length of <see cref="TokenizerContext"/>.
            /// </summary>
            public int Length;

            /// <summary>
            /// Initializes the context.
            /// </summary>
            /// <param name="str"></param>
            public void Initialize(string str)
            {
                Debug.Assert(str != null);

                this.String = str;
                this.Length = str.Length;
                this.Position = 0;
            }

            /// <summary>
            /// Splits current string from current position into tokens using given set of delimiter characters.
            /// Tokenizes the string that was passed to a previous call of <see cref="Initialize"/>.
            /// </summary>
            public string Tokenize(string delimiters)
            {
                if (this.Position >= this.Length) return null;
                if (delimiters == null) delimiters = String.Empty;

                int index;
                char[] delChars = delimiters.ToCharArray();
                while ((index = this.String.IndexOfAny(delChars, this.Position)) == this.Position)
                {
                    if (this.Position == this.Length - 1) return null; // last char is delimiter
                    this.Position++;
                }

                string token;
                if (index == -1) // delimiter not found
                {
                    token = this.String.Substring(this.Position);
                    this.Position = this.Length;
                    return token;
                }

                token = this.String.Substring(this.Position, index - this.Position);
                this.Position = index + 1;
                return token;
            }

            /// <summary>
            /// Empty constructor.
            /// </summary>
            public TokenizerContext() { }
        }

        /// <summary>
        /// Splits a string into tokens using given set of delimiter characters. Tokenizes the string
        /// that was passed to a previous call of the two-parameter version.
        /// </summary>
        /// <param name="ctx">Current runtime context.</param>
        /// <param name="delimiters">Set of delimiters.</param>
        /// <returns>The next token or a <B>null</B> reference.</returns>
        /// <remarks>This method implements the behavior introduced with PHP 4.1.0, i.e. empty tokens are
        /// skipped and never returned.</remarks>
        [return: CastToFalse]
        public static string strtok(Context ctx, string delimiters)
        {
            return ctx.GetStatic<TokenizerContext>().Tokenize(delimiters);
        }

        /// <summary>
        /// Splits a string into tokens using given set of delimiter characters.
        /// </summary>
        /// <param name="ctx">Current runtime context.</param>
        /// <param name="str">The string to tokenize.</param>
        /// <param name="delimiters">Set of delimiters.</param>
        /// <returns>The first token or null. Call one-parameter version of this method to get next tokens.
        /// </returns>
        /// <remarks>This method implements the behavior introduced with PHP 4.1.0, i.e. empty tokens are
        /// skipped and never returned.</remarks>
        [return: CastToFalse]
        public static string strtok(Context ctx, string str, string delimiters)
        {
            if (str == null)
                str = String.Empty;

            var tctx = ctx.GetStatic<TokenizerContext>();
            tctx.Initialize(str);
            return tctx.Tokenize(delimiters);
        }

        #endregion

        #region trim, rtrim, ltrim, chop

        /// <summary>
        /// Strips whitespace characters from the beginning and end of a string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        /// <returns>The trimmed string.</returns>
        /// <remarks>This one-parameter version trims '\0', '\t', '\n', '\r', '\x0b' and ' ' (space).</remarks>
        public static string trim(string str) => trim(str, "\0\t\n\r\x0b\x20");

        /// <summary>
        /// Strips given characters from the beginning and end of a string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        /// <param name="whiteSpaceCharacters">The characters to strip from <paramref name="str"/>. Can contain ranges
        /// of characters, e.g. "\0x00..\0x1F".</param>
        /// <returns>The trimmed string.</returns>
        /// <exception cref="PhpException"><paramref name="whiteSpaceCharacters"/> is invalid char mask. Multiple errors may be printed out.</exception>
        /// <exception cref="PhpException"><paramref name="str"/> contains Unicode characters greater than '\u0800'.</exception>
        public static string trim(string str, string whiteSpaceCharacters)
        {
            if (str == null) return String.Empty;

            // As whiteSpaceCharacters may contain intervals, I see two possible implementations:
            // 1) Call CharMap.AddUsingMask and do the trimming "by hand".
            // 2) Write another version of CharMap.AddUsingMask that would return char[] of characters
            // that fit the mask, and do the trimming with String.Trim(char[]).
            // I have chosen 1).

            CharMap charmap = InitializeCharMap();

            // may throw an exception:
            try
            {
                charmap.AddUsingMask(whiteSpaceCharacters);
            }
            catch (IndexOutOfRangeException)
            {
                //PhpException.Throw(PhpError.Warning, LibResources.GetString("unicode_characters"));
                //return null;
                throw;
            }

            int length = str.Length, i = 0, j = length - 1;

            // finds the new beginning:
            while (i < length && charmap.Contains(str[i])) i++;

            // finds the new end:
            while (j >= 0 && charmap.Contains(str[j])) j--;

            return (i <= j) ? str.Substring(i, j - i + 1) : String.Empty;
        }

        /// <summary>Characters treated as blanks by the PHP.</summary>
        private static char[] phpBlanks = new char[] { '\0', '\t', '\n', '\r', '\u000b', ' ' };

        /// <summary>
        /// Strips whitespace characters from the beginning of a string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        /// <returns>The trimmed string.</returns>
        /// <remarks>This one-parameter version trims '\0', '\t', '\n', '\r', '\u000b' and ' ' (space).</remarks>
        public static string ltrim(string str)
        {
            return (str != null) ? str.TrimStart(phpBlanks) : String.Empty;
        }

        /// <summary>
        /// Strips given characters from the beginning of a string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        /// <param name="whiteSpaceCharacters">The characters to strip from <paramref name="str"/>. Can contain ranges
        /// of characters, e.g. \0x00..\0x1F.</param>
        /// <returns>The trimmed string.</returns>
        /// <exception cref="PhpException"><paramref name="whiteSpaceCharacters"/> is invalid char mask. Multiple errors may be printed out.</exception>
        /// <exception cref="PhpException"><paramref name="whiteSpaceCharacters"/> contains Unicode characters greater than '\u0800'.</exception>
        public static string ltrim(string str, string whiteSpaceCharacters)
        {
            if (str == null) return String.Empty;

            CharMap charmap = InitializeCharMap();

            // may throw an exception:
            try
            {
                charmap.AddUsingMask(whiteSpaceCharacters);
            }
            catch (IndexOutOfRangeException)
            {
                //PhpException.Throw(PhpError.Warning, LibResources.GetString("unicode_characters"));
                //return null;
                throw;
            }

            int length = str.Length, i = 0;

            while (i < length && charmap.Contains(str[i])) i++;

            if (i < length) return str.Substring(i);
            return String.Empty;
        }

        /// <summary>
        /// Strips whitespace characters from the end of a string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        /// <returns>The trimmed string.</returns>
        /// <remarks>This one-parameter version trims '\0', '\t', '\n', '\r', '\u000b' and ' ' (space).</remarks>
        public static string rtrim(string str)
        {
            return (str != null) ? str.TrimEnd(phpBlanks) : String.Empty;
        }

        /// <summary>
        /// Strips given characters from the end of a string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        /// <param name="whiteSpaceCharacters">The characters to strip from <paramref name="str"/>. Can contain ranges
        /// of characters, e.g. \0x00..\0x1F.</param>
        /// <returns>The trimmed string.</returns>
        /// <exception cref="PhpException"><paramref name="whiteSpaceCharacters"/> is invalid char mask. Multiple errors may be printed out.</exception>
        /// <exception cref="PhpException"><paramref name="whiteSpaceCharacters"/> contains Unicode characters greater than '\u0800'.</exception>
        public static string rtrim(string str, string whiteSpaceCharacters)
        {
            if (str == null) return String.Empty;

            CharMap charmap = InitializeCharMap();

            try
            {
                charmap.AddUsingMask(whiteSpaceCharacters);
            }
            catch (IndexOutOfRangeException)
            {
                //PhpException.Throw(PhpError.Warning, LibResources.GetString("unicode_characters"));
                //return null;
                throw;
            }

            int j = str.Length - 1;

            while (j >= 0 && charmap.Contains(str[j])) j--;

            return (j >= 0) ? str.Substring(0, j + 1) : String.Empty;
        }

        /// <summary>
        /// Strips whitespace characters from the end of a string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        /// <returns>The trimmed string.</returns>
        /// <remarks>This one-parameter version trims '\0', '\t', '\n', '\r', '\u000b' and ' ' (space).</remarks>
        public static string chop(string str)
        {
            return rtrim(str);
        }

        /// <summary>
        /// Strips given characters from the end of a string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        /// <param name="whiteSpaceCharacters">The characters to strip from <paramref name="str"/>. Can contain ranges
        /// of characters, e.g. \0x00..\0x1F.</param>
        /// <returns>The trimmed string.</returns>
        /// <exception cref="PhpException">Thrown if <paramref name="whiteSpaceCharacters"/> is invalid char mask. Multiple errors may be printed out.</exception>
        public static string chop(string str, string whiteSpaceCharacters)
        {
            return rtrim(str, whiteSpaceCharacters);
        }

        #endregion

        #region ucfirst, lcfirst, ucwords

        /// <summary>
        /// Makes a string's first character uppercase.
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <returns><paramref name="str"/> with the first character converted to uppercase.</returns>
        public static string ucfirst(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            return Char.ToUpper(str[0]) + str.Substring(1);
        }

        /// <summary>
        /// Returns a string with the first character of str , lowercased if that character is alphabetic.
        /// Note that 'alphabetic' is determined by the current locale. For instance, in the default "C" locale characters such as umlaut-a (ä) will not be converted. 
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <returns>Returns the resulting string.</returns>
        public static string lcfirst(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            // first character to lower case
            return char.ToLower(str[0]) + str.Substring(1);
        }

        /// <summary>
        /// Makes the first character of each word in a string uppercase.
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <returns><paramref name="str"/> with the first character of each word in a string converted to 
        /// uppercase.</returns>
        public static string ucwords(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            int length = str.Length;
            var result = new StringBuilder(str);

            bool state = true;
            for (int i = 0; i < length; i++)
            {
                if (char.IsWhiteSpace(result[i])) state = true;
                else
                {
                    if (state)
                    {
                        result[i] = char.ToUpper(result[i]);
                        state = false;
                    }
                }
            }

            return result.ToString();
        }

        #endregion

        #region sprintf, vsprintf

        /// <summary>
        /// Default number of decimals when formatting floating-point numbers (%f in printf).
        /// </summary>
        internal const int printfFloatPrecision = 6;

        /// <summary>
        /// Returns a formatted string.
        /// </summary>
        /// <param name="ctx">Current runtime context.</param>
        /// <param name="format">The format string. 
        /// See <A href="http://www.php.net/manual/en/function.sprintf.php">PHP manual</A> for details.
        /// Besides, a type specifier "%C" is applicable. It converts an integer value to Unicode character.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The formatted string or null if there is too few arguments.</returns>
        /// <remarks>Assumes that either <paramref name="format"/> nor <paramref name="arguments"/> is null.</remarks>
        internal static string FormatInternal(Context ctx, string format, PhpValue[] arguments)
        {
            Debug.Assert(format != null && arguments != null);

            Encoding encoding = ctx.StringEncoding;
            StringBuilder result = new StringBuilder();
            int state = 0, width = 0, precision = -1, seqIndex = 0, swapIndex = -1;
            bool leftAlign = false;
            bool plusSign = false;
            char padChar = ' ';

            // process the format string using a 6-state finite automaton
            int length = format.Length;
            for (int i = 0; i < length; i++)
            {
                char c = format[i];

                Lambda:
                switch (state)
                {
                    case 0: // the initial state
                        {
                            if (c == '%')
                            {
                                width = 0;
                                precision = -1;
                                swapIndex = -1;
                                leftAlign = false;
                                plusSign = false;
                                padChar = ' ';
                                state = 1;
                            }
                            else result.Append(c);
                            break;
                        }

                    case 1: // % character encountered, expecting format
                        {
                            switch (c)
                            {
                                case '-': leftAlign = true; break;
                                case '+': plusSign = true; break;
                                case ' ': padChar = ' '; break;
                                case '\'': state = 2; break;
                                case '.': state = 4; break;
                                case '%': result.Append(c); state = 0; break;
                                case '0': padChar = '0'; state = 3; break;

                                default:
                                    {
                                        if (Char.IsDigit(c)) state = 3;
                                        else state = 5;
                                        goto Lambda;
                                    }
                            }
                            break;
                        }

                    case 2: // ' character encountered, expecting padding character
                        {
                            padChar = c;
                            state = 1;
                            break;
                        }

                    case 3: // number encountered, expecting width or argument number
                        {
                            switch (c)
                            {
                                case '$':
                                    {
                                        swapIndex = width;
                                        if (swapIndex == 0)
                                        {
                                            //PhpException.Throw(PhpError.Warning, LibResources.GetString("zero_argument_invalid"));
                                            //return result.ToString();
                                            throw new ArgumentException();
                                        }

                                        width = 0;
                                        state = 1;
                                        break;
                                    }

                                case '.':
                                    {
                                        state = 4;
                                        break;
                                    }

                                default:
                                    {
                                        if (Char.IsDigit(c)) width = width * 10 + (int)Char.GetNumericValue(c);
                                        else
                                        {
                                            state = 5;
                                            goto Lambda;
                                        }
                                        break;
                                    }
                            }
                            break;
                        }

                    case 4: // number after . encountered, expecting precision
                        {
                            if (precision == -1) precision = 0;
                            if (Char.IsDigit(c)) precision = precision * 10 + (int)Char.GetNumericValue(c);
                            else
                            {
                                state = 5;
                                goto case 5;
                            }
                            break;
                        }

                    case 5: // expecting type specifier
                        {
                            int index = (swapIndex <= 0 ? seqIndex++ : swapIndex - 1);
                            if (index >= arguments.Length)
                            {
                                // few arguments:
                                return null;
                            }

                            var obj = arguments[index];
                            string app = null;
                            char sign = '\0';

                            switch (c)
                            {
                                case 'b': // treat as integer, present as binary number without a sign
                                    app = System.Convert.ToString(obj.ToLong(), 2);
                                    break;

                                case 'c': // treat as integer, present as character
                                    app = encoding.GetString(new byte[] { unchecked((byte)obj.ToLong()) }, 0, 1);
                                    break;

                                case 'C': // treat as integer, present as Unicode character
                                    app = new String(unchecked((char)obj.ToLong()), 1);
                                    break;

                                case 'd': // treat as integer, present as signed decimal number
                                    {
                                        // use long to prevent overflow in Math.Abs:
                                        long ivalue = obj.ToLong();
                                        if (ivalue < 0) sign = '-'; else if (ivalue >= 0 && plusSign) sign = '+';

                                        app = Math.Abs((long)ivalue).ToString();
                                        break;
                                    }

                                case 'u': // treat as integer, present as unsigned decimal number, without sign
                                    app = unchecked((uint)obj.ToLong()).ToString();
                                    break;

                                case 'e':
                                    {
                                        double dvalue = obj.ToDouble();
                                        if (dvalue < 0) sign = '-'; else if (dvalue >= 0 && plusSign) sign = '+';

                                        string f = String.Concat("0.", new String('0', precision == -1 ? printfFloatPrecision : precision), "e+0");
                                        app = Math.Abs(dvalue).ToString(f);
                                        break;
                                    }

                                case 'f': // treat as float, present locale-aware floating point number
                                    {
                                        double dvalue = obj.ToDouble();
                                        if (dvalue < 0) sign = '-'; else if (dvalue >= 0 && plusSign) sign = '+';

                                        app = Math.Abs(dvalue).ToString("F" + (precision == -1 ? printfFloatPrecision : precision));
                                        break;
                                    }

                                case 'F': // treat as float, present locale-unaware floating point number with '.' decimal separator (PHP 5.0.3+ feature)
                                    {
                                        double dvalue = obj.ToDouble();
                                        if (dvalue < 0) sign = '-'; else if (dvalue >= 0 && plusSign) sign = '+';

                                        app = Math.Abs(dvalue).ToString("F" + (precision == -1 ? printfFloatPrecision : precision),
                                          System.Globalization.NumberFormatInfo.InvariantInfo);
                                        break;
                                    }

                                case 'o': // treat as integer, present as octal number without sign
                                    app = System.Convert.ToString(obj.ToLong(), 8);
                                    break;

                                case 'x': // treat as integer, present as hex number (lower case) without sign
                                    app = obj.ToLong().ToString("x");
                                    break;

                                case 'X': // treat as integer, present as hex number (upper case) without sign
                                    app = obj.ToLong().ToString("X");
                                    break;

                                case 's': // treat as string, present as string
                                    app = obj.ToString(ctx);

                                    // undocumented feature:
                                    if (precision != -1) app = app.Remove(Math.Min(precision, app.Length));

                                    break;
                            }

                            if (app != null)
                            {
                                // pad:
                                if (leftAlign)
                                {
                                    if (sign != '\0') result.Append(sign);
                                    result.Append(app);
                                    for (int j = width - app.Length; j > ((sign != '\0') ? 1 : 0); j--)
                                        result.Append(padChar);
                                }
                                else
                                {
                                    if (sign != '\0' && padChar == '0')
                                        result.Append(sign);

                                    for (int j = width - app.Length; j > ((sign != '\0') ? 1 : 0); j--)
                                        result.Append(padChar);

                                    if (sign != '\0' && padChar != '0')
                                        result.Append(sign);

                                    result.Append(app);
                                }
                            }

                            state = 0;
                            break;
                        }
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Returns a formatted string.
        /// </summary>
        /// <param name="ctx">Current runtime context.</param>
        /// <param name="format">The format string. For details, see PHP manual.</param>
        /// <param name="arguments">The arguments.
        /// See <A href="http://www.php.net/manual/en/function.sprintf.php">PHP manual</A> for details.
        /// Besides, a type specifier "%C" is applicable. It converts an integer value to Unicode character.</param>
        /// <returns>The formatted string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="arguments"/> parameter is null.</exception>
        /// <exception cref="PhpException">Thrown when there is less arguments than expeceted by formatting string.</exception>
        [return: CastToFalse]
        public static string sprintf(Context ctx, string format, params PhpValue[] arguments)
        {
            if (format == null) return string.Empty;

            // null arguments would be compiler's error (or error of the user):
            if (arguments == null) throw new ArgumentNullException("arguments");

            var result = FormatInternal(ctx, format, arguments);
            if (result == null)
            {
                //PhpException.Throw(PhpError.Warning, LibResources.GetString("too_few_arguments"));

                // TODO: return FALSE
                throw new ArgumentException();
            }
            return result;
        }

        /// <summary>
        /// Returns a formatted string.
        /// </summary>
        /// <param name="ctx">Current runtime context.</param>
        /// <param name="format">The format string. For details, see PHP manual.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The formatted string.</returns>
        /// <exception cref="PhpException">Thrown when there is less arguments than expeceted by formatting string.</exception>
        [return: CastToFalse]
        public static string vsprintf(Context ctx, string format, PhpArray arguments)
        {
            if (format == null) return string.Empty;

            PhpValue[] array;
            if (arguments != null && arguments.Count != 0)
            {
                array = new PhpValue[arguments.Count];
                arguments.Values.CopyTo(array, 0);
            }
            else
            {
                array = Core.Utilities.ArrayUtils.EmptyValues;
            }

            var result = FormatInternal(ctx, format, array);
            if (result == null)
            {
                //PhpException.Throw(PhpError.Warning, LibResources.GetString("too_few_arguments"));

                // TODO: return FALSE
                throw new ArgumentException();
            }
            return result;
        }

        #endregion

        #region wordwrap

        /// <summary>
        /// Wraps a string to a specified number of characters using a specified string as the break string.
        /// </summary>
        /// <param name="str">The string to word-wrap.</param>
        /// <param name="width">The desired line length.</param>
        /// <param name="lineBreak">The break string.</param>
        /// <param name="cut">If true, words longer than <paramref name="width"/> will be cut so that no line is longer
        /// than <paramref name="width"/>.</param>
        /// <returns>The word-wrapped string.</returns>
        /// <remarks>The only "break-point" character is space (' ').</remarks>
        /// <exception cref="PhpException">Thrown if the combination of <paramref name="width"/> and <paramref name="cut"/> is invalid.</exception>
        [return: CastToFalse]
        public static string wordwrap(string str, int width = 75, string lineBreak = "\n", bool cut = false)
        {
            if (width == 0 && cut)
            {
                //PhpException.Throw(PhpError.Warning, LibResources.GetString("cut_forced_with_zero_width"));
                //return null;
                throw new ArgumentException();
            }
            if (str == null) return null;

            int length = str.Length;
            StringBuilder result = new StringBuilder(length);

            // mimic the strange PHP behaviour when width < 0 and cut is true
            if (width < 0 && cut)
            {
                result.Append(lineBreak);
                width = 1;
            }

            int lastSpace = -1, lineStart = 0;
            for (int i = 0; i < length; i++)
            {
                if (str[i] == ' ')
                {
                    lastSpace = i;
                    if (i - lineStart >= width + 1)
                    {
                        // cut is false if we get here
                        if (lineStart == 0)
                        {
                            result.Append(str, 0, i);
                        }
                        else
                        {
                            result.Append(lineBreak);
                            result.Append(str, lineStart, i - lineStart);
                        }

                        lineStart = i + 1;
                        continue;
                    }
                }

                if (i - lineStart >= width)
                {
                    // we reached the specified width

                    if (lastSpace > lineStart) // obsolete: >=
                    {
                        if (lineStart > 0) result.Append(lineBreak);
                        result.Append(str, lineStart, lastSpace - lineStart);
                        lineStart = lastSpace + 1;
                    }
                    else if (cut)
                    {
                        if (lineStart > 0) result.Append(lineBreak);
                        result.Append(str, lineStart, width);
                        lineStart = i;
                    }
                }
            }

            // process the rest of str
            if (lineStart < length || lastSpace == length - 1)
            {
                if (lineStart > 0) result.Append(lineBreak);
                result.Append(str, lineStart, length - lineStart);
            }

            return result.ToString();
        }

        #endregion

        #region number_format, money_format

        /// <summary>
        /// Formats a number with grouped thousands.
        /// </summary>
        /// <param name="number">The number to format.</param>
        /// <returns>String representation of the number without decimals (rounded) with comma between every group
        /// of thousands.</returns>
        public static string number_format(double number)
        {
            return number_format(number, 0, ".", ",");
        }

        /// <summary>
        /// Formats a number with grouped thousands and with given number of decimals.
        /// </summary>
        /// <param name="number">The number to format.</param>
        /// <param name="decimals">The number of decimals.</param>
        /// <returns>String representation of the number with <paramref name="decimals"/> decimals with a dot in front, and with 
        /// comma between every group of thousands.</returns>
        public static string number_format(double number, int decimals)
        {
            return number_format(number, decimals, ".", ",");
        }

        /// <summary>
        /// Formats a number with grouped thousands, with given number of decimals, with given decimal point string
        /// and with given thousand separator.
        /// </summary>
        /// <param name="number">The number to format.</param>
        /// <param name="decimals">The number of decimals within range 0 to 99.</param>
        /// <param name="decimalPoint">The string to separate integer part and decimals.</param>
        /// <param name="thousandsSeparator">The character to separate groups of thousands. Only the first character
        /// of <paramref name="thousandsSeparator"/> is used.</param>
        /// <returns>
        /// String representation of the number with <paramref name="decimals"/> decimals with <paramref name="decimalPoint"/> in 
        /// front, and with <paramref name="thousandsSeparator"/> between every group of thousands.
        /// </returns>
        /// <remarks>
        /// The <b>number_format</b> (<see cref="FormatNumber"/>) PHP function requires <paramref name="decimalPoint"/> and <paramref name="thousandsSeparator"/>
        /// to be of length 1 otherwise it uses default values (dot and comma respectively). As this behavior does
        /// not make much sense, this method has no such limitation except for <paramref name="thousandsSeparator"/> of which
        /// only the first character is used (documented feature).
        /// </remarks>
        public static string number_format(double number, int decimals, string decimalPoint, string thousandsSeparator)
        {
            System.Globalization.NumberFormatInfo format = new System.Globalization.NumberFormatInfo();

            if ((decimals >= 0) && (decimals <= 99))
            {
                format.NumberDecimalDigits = decimals;
            }
            else
            {
                //PhpException.InvalidArgument("decimals", LibResources.GetString("arg_out_of_bounds", decimals));
                throw new ArgumentException();
            }

            if (!string.IsNullOrEmpty(decimalPoint))
            {
                format.NumberDecimalSeparator = decimalPoint;
            }

            if (thousandsSeparator == null) thousandsSeparator = String.Empty;

            switch (thousandsSeparator.Length)
            {
                case 0: format.NumberGroupSeparator = String.Empty; break;
                case 1: format.NumberGroupSeparator = thousandsSeparator; break;
                default: format.NumberGroupSeparator = thousandsSeparator.Substring(0, 1); break;
            }

            return number.ToString("N", format);
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        public static string money_format(string format, double number)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region hebrev, hebrevc

        /// <summary>
        /// Indicates whether a character is recognized as Hebrew letter.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <returns>
        /// Whether the <paramref name="c"/> is a Hebrew letter according to 
        /// the <A href="http://www.unicode.org/charts/PDF/U0590.pdf">Unicode 4.0 standard</A>.
        /// </returns>
        internal static bool IsHebrew(char c)
        {
            return c >= '\u05d0' && c <= '\u05ea';
        }

        /// <summary>
        /// Indicates whether a character is a space or tab.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <returns>True iff space or tab.</returns>
        internal static bool IsBlank(char c)
        {
            return c == ' ' || c == '\t';
        }

        /// <summary>
        /// Indicates whether a character is new line or carriage return.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <returns>True iff new line or carriage return.</returns>
        internal static bool IsNewLine(char c)
        {
            return c == '\n' || c == '\r';
        }

        /// <summary>
        /// Converts logical Hebrew text to visual text.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="maxCharactersPerLine">If &gt;0, maximum number of characters per line. If 0,
        /// there is no maximum.</param>
        /// <param name="convertNewLines">Whether to convert new lines '\n' to "&lt;br/&gt;".</param>
        /// <returns>The converted string.</returns>
        internal static string HebrewReverseInternal(string str, int maxCharactersPerLine, bool convertNewLines)
        {
            if (str == null || str == String.Empty) return str;
            int length = str.Length, blockLength = 0, blockStart = 0, blockEnd = 0;

            StringBuilder hebStr = new StringBuilder(length);
            hebStr.Length = length;

            bool blockTypeHeb = IsHebrew(str[0]);
            int source = 0, target = length - 1;

            do
            {
                if (blockTypeHeb)
                {
                    while (source + 1 < length && (IsHebrew(str[source + 1]) || IsBlank(str[source + 1]) ||
                        Char.IsPunctuation(str[source + 1]) || str[source + 1] == '\n') && blockEnd < length - 1)
                    {
                        source++;
                        blockEnd++;
                        blockLength++;
                    }

                    for (int i = blockStart; i <= blockEnd; i++)
                    {
                        switch (str[i])
                        {
                            case '(': hebStr[target] = ')'; break;
                            case ')': hebStr[target] = '('; break;
                            case '[': hebStr[target] = ']'; break;
                            case ']': hebStr[target] = '['; break;
                            case '{': hebStr[target] = '}'; break;
                            case '}': hebStr[target] = '{'; break;
                            case '<': hebStr[target] = '>'; break;
                            case '>': hebStr[target] = '<'; break;
                            case '\\': hebStr[target] = '/'; break;
                            case '/': hebStr[target] = '\\'; break;
                            default: hebStr[target] = str[i]; break;
                        }
                        target--;
                    }
                    blockTypeHeb = false;
                }
                else
                {
                    // blockTypeHeb == false

                    while (source + 1 < length && !IsHebrew(str[source + 1]) && str[source + 1] != '\n' &&
                        blockEnd < length - 1)
                    {
                        source++;
                        blockEnd++;
                        blockLength++;
                    }
                    while ((IsBlank(str[source]) || Char.IsPunctuation(str[source])) && str[source] != '/' &&
                        str[source] != '-' && blockEnd > blockStart)
                    {
                        source--;
                        blockEnd--;
                    }
                    for (int i = blockEnd; i >= blockStart; i--)
                    {
                        hebStr[target] = str[i];
                        target--;
                    }
                    blockTypeHeb = true;
                }

                blockStart = blockEnd + 1;

            } while (blockEnd < length - 1);

            StringBuilder brokenStr = new StringBuilder(length);
            brokenStr.Length = length;
            int begin = length - 1, end = begin, charCount, origBegin;
            target = 0;

            while (true)
            {
                charCount = 0;
                while ((maxCharactersPerLine == 0 || charCount < maxCharactersPerLine) && begin > 0)
                {
                    charCount++;
                    begin--;
                    if (begin <= 0 || IsNewLine(hebStr[begin]))
                    {
                        while (begin > 0 && IsNewLine(hebStr[begin - 1]))
                        {
                            begin--;
                            charCount++;
                        }
                        break;
                    }
                }

                if (charCount == maxCharactersPerLine)
                {
                    // try to avoid breaking words
                    int newCharCount = charCount, newBegin = begin;

                    while (newCharCount > 0)
                    {
                        if (IsBlank(hebStr[newBegin]) || IsNewLine(hebStr[newBegin])) break;

                        newBegin++;
                        newCharCount--;
                    }
                    if (newCharCount > 0)
                    {
                        charCount = newCharCount;
                        begin = newBegin;
                    }
                }
                origBegin = begin;

                if (IsBlank(hebStr[begin])) hebStr[begin] = '\n';

                while (begin <= end && IsNewLine(hebStr[begin]))
                {
                    // skip leading newlines
                    begin++;
                }

                for (int i = begin; i <= end; i++)
                {
                    // copy content
                    brokenStr[target] = hebStr[i];
                    target++;
                }

                for (int i = origBegin; i <= end && IsNewLine(hebStr[i]); i++)
                {
                    brokenStr[target] = hebStr[i];
                    target++;
                }

                begin = origBegin;
                if (begin <= 0) break;

                begin--;
                end = begin;
            }

            if (convertNewLines) brokenStr.Replace("\n", "<br/>\n");
            return brokenStr.ToString();
        }

        /// <summary>
        /// Converts logical Hebrew text to visual text.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The comverted string.</returns>
        /// <remarks>Although PHP returns false if <paramref name="str"/> is null or empty there is no reason to do so.</remarks>
        public static string hebrev(string str)
        {
            return HebrewReverseInternal(str, 0, false);
        }

        /// <summary>
        /// Converts logical Hebrew text to visual text.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="maxCharactersPerLine">Maximum number of characters per line.</param>
        /// <returns>The comverted string.</returns>
        /// <remarks>Although PHP returns false if <paramref name="str"/> is null or empty there is no reason to do so.</remarks>
        public static string hebrev(string str, int maxCharactersPerLine)
        {
            return HebrewReverseInternal(str, maxCharactersPerLine, false);
        }

        /// <summary>
        /// Converts logical Hebrew text to visual text and also converts new lines '\n' to "&lt;br/&gt;".
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The converted string.</returns>
        /// <remarks>Although PHP returns false if <paramref name="str"/> is null or empty there is no reason to do so.</remarks>
        public static string hebrevc(string str)
        {
            return HebrewReverseInternal(str, 0, true);
        }

        /// <summary>
        /// Converts logical Hebrew text to visual text and also converts new lines '\n' to "&lt;br/&gt;".
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="maxCharactersPerLine">Maximum number of characters per line.</param>
        /// <returns>The comverted string.</returns>
        /// <remarks>Although PHP returns false if <paramref name="str"/> is null or empty there is no reason to do so.</remarks>
        public static string hebrevc(string str, int maxCharactersPerLine)
        {
            return HebrewReverseInternal(str, maxCharactersPerLine, true);
        }

        #endregion

        #region str_pad

        /// <summary>
        /// Type of padding.
        /// </summary>
        public enum PaddingType
        {
            /// <summary>Pad a string from the left.</summary>
            Left = 0,

            /// <summary>Pad a string from the right.</summary>
            Right = 1,

            /// <summary>Pad a string from both sides.</summary>
            Both = 2
        }

        /// <summary>
        /// Pad a string from the left.
        /// </summary>
        public const int STR_PAD_LEFT = (int)PaddingType.Left;

        /// <summary>
        /// Pad a string from the right.
        /// </summary>
        public const int STR_PAD_RIGHT = (int)PaddingType.Right;

        /// <summary>
        /// Pad a string from both sides.
        /// </summary>
        public const int STR_PAD_BOTH = (int)PaddingType.Both;

        /// <summary>
        /// Pads a string to certain length with another string.
        /// </summary>
        /// <param name="str">The string to pad.</param>
        /// <param name="totalWidth">Desired length of the returned string.</param>
        /// <param name="paddingString">The string to use as the pad.</param>
        /// <param name="paddingType">Specifies whether the padding should be done on the left, on the right,
        /// or on both sides of <paramref name="str"/>.</param>
        /// <returns><paramref name="str"/> padded with <paramref name="paddingString"/>.</returns>
        /// <exception cref="PhpException">Thrown if <paramref name="paddingType"/> is invalid or <paramref name="paddingString"/> is null or empty.</exception>
        public static string str_pad(string str, int totalWidth, string paddingString = " ", PaddingType paddingType = PaddingType.Right)
        {
            //PhpBytes binstr = str as PhpBytes;
            //if (str is PhpBytes)
            //{
            //    PhpBytes binPaddingString = Core.Convert.ObjectToPhpBytes(paddingString);

            //    if (binPaddingString == null || binPaddingString.Length == 0)
            //    {
            //        PhpException.InvalidArgument("paddingString", LibResources.GetString("arg_null_or_empty"));
            //        return null;
            //    }
            //    if (binstr == null) binstr = PhpBytes.Empty;

            //    int length = binstr.Length;
            //    if (totalWidth <= length) return binstr;

            //    int pad = totalWidth - length, padLeft = 0, padRight = 0;

            //    switch (paddingType)
            //    {
            //        case PaddingType.Left: padLeft = pad; break;
            //        case PaddingType.Right: padRight = pad; break;

            //        case PaddingType.Both:
            //            padLeft = pad / 2;
            //            padRight = pad - padLeft;
            //            break;

            //        default:
            //            PhpException.InvalidArgument("paddingType");
            //            break;
            //    }

            //    // if paddingString has length 1, use String.PadLeft and String.PadRight
            //    int padStrLength = binPaddingString.Length;

            //    // else build the resulting string manually
            //    byte[] result = new byte[totalWidth];

            //    int position = 0;

            //    // pad left
            //    while (padLeft > padStrLength)
            //    {
            //        Buffer.BlockCopy(binPaddingString.ReadonlyData, 0, result, position, padStrLength);
            //        padLeft -= padStrLength;
            //        position += padStrLength;
            //    }

            //    if (padLeft > 0)
            //    {
            //        Buffer.BlockCopy(binPaddingString.ReadonlyData, 0, result, position, padLeft);
            //        position += padLeft;
            //    }

            //    Buffer.BlockCopy(binstr.ReadonlyData, 0, result, position, binstr.Length);
            //    position += binstr.Length;

            //    // pad right
            //    while (padRight > padStrLength)
            //    {
            //        Buffer.BlockCopy(binPaddingString.ReadonlyData, 0, result, position, padStrLength);
            //        padRight -= padStrLength;
            //        position += padStrLength;
            //    }

            //    if (padRight > 0)
            //    {
            //        Buffer.BlockCopy(binPaddingString.ReadonlyData, 0, result, position, padRight);
            //        position += padRight;
            //    }

            //    return new PhpBytes(result);
            //}

            string unistr = str; // Core.Convert.ObjectToString(str);
            if (unistr != null)
            {
                string uniPaddingString = paddingString; // Core.Convert.ObjectToString(paddingString);

                if (string.IsNullOrEmpty(uniPaddingString))
                {
                    //PhpException.InvalidArgument("paddingString", LibResources.GetString("arg_null_or_empty"));
                    //return null;
                    throw new ArgumentException();
                }

                int length = unistr.Length;
                if (totalWidth <= length) return unistr;

                int pad = totalWidth - length, padLeft = 0, padRight = 0;

                switch (paddingType)
                {
                    case PaddingType.Left: padLeft = pad; break;
                    case PaddingType.Right: padRight = pad; break;

                    case PaddingType.Both:
                        padLeft = pad / 2;
                        padRight = pad - padLeft;
                        break;

                    default:
                        //PhpException.InvalidArgument("paddingType");
                        //break;
                        throw new ArgumentException();
                }

                // if paddingString has length 1, use String.PadLeft and String.PadRight
                int padStrLength = uniPaddingString.Length;
                if (padStrLength == 1)
                {
                    char c = uniPaddingString[0];
                    if (padLeft > 0) unistr = unistr.PadLeft(length + padLeft, c);
                    if (padRight > 0) unistr = unistr.PadRight(totalWidth, c);

                    return unistr;
                }

                // else build the resulting string manually
                StringBuilder result = new StringBuilder(totalWidth);

                // pad left
                while (padLeft > padStrLength)
                {
                    result.Append(uniPaddingString);
                    padLeft -= padStrLength;
                }
                if (padLeft > 0) result.Append(uniPaddingString.Substring(0, padLeft));

                result.Append(unistr);

                // pad right
                while (padRight > padStrLength)
                {
                    result.Append(uniPaddingString);
                    padRight -= padStrLength;
                }
                if (padRight > 0) result.Append(uniPaddingString.Substring(0, padRight));

                return result.ToString();
            }

            return null;
        }

        #endregion

        #region str_word_count

        /// <summary>
        /// Format of a return value of <see cref="PhpStrings.CountWords"/> method. Constants are not named in PHP.
        /// </summary>                   
        public enum WordCountResult
        {
            /// <summary>
            /// Return number of words in string.
            /// </summary>
            WordCount = 0,

            /// <summary>
            /// Return array of words.
            /// </summary>
            WordsArray = 1,

            /// <summary>
            /// Return positions to words mapping.
            /// </summary>
            PositionsToWordsMapping = 2
        }

        /// <summary>
        /// Counts the number of words inside a string.
        /// </summary>
        /// <param name="str">The string containing words to count.</param>
        /// <returns>Then number of words inside <paramref name="str"/>. </returns>
        public static int str_word_count(string str)
        {
            return CountWords(str, WordCountResult.WordCount, null, null);
        }

        /// <summary>
        /// Counts the number of words inside a string.
        /// </summary>
        public static PhpValue str_word_count(string str, WordCountResult format = WordCountResult.WordCount, string addWordChars = null)
        {
            PhpArray words = (format != WordCountResult.WordCount) ? new PhpArray() : null;

            int count = CountWords(str, format, addWordChars, words);

            if (count == -1)
                return PhpValue.False;

            if (format == WordCountResult.WordCount)
                return PhpValue.Create(count);
            else
            {
                if (words != null)
                    return PhpValue.Create(words);
                else
                    return PhpValue.False;
            }
        }

        private static bool IsWordChar(char c, CharMap map)
        {
            return char.IsLetter(c) || map != null && map.Contains(c);
        }

        internal static int CountWords(string str, WordCountResult format, string addWordChars, IDictionary words)
        {
            if (str == null)
                return 0;
            if (format != WordCountResult.WordCount && words == null)
                throw new ArgumentNullException("words");

            CharMap charmap = null;

            if (!String.IsNullOrEmpty(addWordChars))
            {
                charmap = InitializeCharMap();
                charmap.Add(addWordChars);
            }

            // find the end
            int last = str.Length - 1;
            if (last > 0 && str[last] == '-' && !IsWordChar(str[last], charmap)) last--;

            // find the beginning
            int pos = 0;
            if (last >= 0 && (str[0] == '-' || str[0] == '\'') && !IsWordChar(str[0], charmap)) pos++;

            int word_count = 0;

            while (pos <= last)
            {
                if (IsWordChar(str[pos], charmap) || str[pos] == '\'' || str[pos] == '-')
                {
                    // word started - read it whole:
                    int word_start = pos++;
                    while (pos <= last &&
                        (IsWordChar(str[pos], charmap) ||
                         str[pos] == '\'' || str[pos] == '-'))
                    {
                        pos++;
                    }

                    switch (format)
                    {
                        case WordCountResult.WordCount:
                            break;

                        case WordCountResult.WordsArray:
                            words.Add(word_count, str.Substring(word_start, pos - word_start));
                            break;

                        case WordCountResult.PositionsToWordsMapping:
                            words.Add(word_start, str.Substring(word_start, pos - word_start));
                            break;

                        default:
                            //PhpException.InvalidArgument("format");
                            //return -1;
                            throw new ArgumentException();
                    }

                    word_count++;
                }
                else pos++;
            }
            return word_count;
        }

        #endregion

        #region strcmp, strcasecmp, strncmp, strncasecmp

        /// <summary>
        /// Compares two specified strings, honoring their case, using culture invariant comparison.
        /// </summary>
        /// <param name="str1">A string.</param>
        /// <param name="str2">A string.</param>
        /// <returns>Returns -1 if <paramref name="str1"/> is less than <paramref name="str2"/>; +1 if <paramref name="str1"/> is greater than <paramref name="str2"/>,
        /// and 0 if they are equal.</returns>
        public static int strcmp(string str1, string str2) => string.CompareOrdinal(str1, str2);

        /// <summary>
        /// Compares two specified strings, ignoring their case, using culture invariant comparison.
        /// </summary>
        /// <param name="str1">A string.</param>
        /// <param name="str2">A string.</param>
        /// <returns>Returns -1 if <paramref name="str1"/> is less than <paramref name="str2"/>; +1 if <paramref name="str1"/> is greater than <paramref name="str2"/>,
        /// and 0 if they are equal.</returns>
        public static int strcasecmp(string str1, string str2)
        {
            return System.Globalization.CultureInfo.InvariantCulture.CompareInfo
                .Compare(str1, str2, System.Globalization.CompareOptions.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Compares parts of two specified strings, honoring their case, using culture invariant comparison.
        /// </summary>
        /// <param name="str1">The lesser string.</param>
        /// <param name="str2">The greater string.</param>
        /// <param name="length">The upper limit of the length of parts to be compared.</param>
        /// <returns>Returns -1 if <paramref name="str1"/> is less than <paramref name="str2"/>; +1 if <paramref name="str1"/> is greater than <paramref name="str2"/>,
        /// and 0 if they are equal.</returns>
        public static PhpValue strncmp(string str1, string str2, int length)
        {
            if (length < 0)
            {
                throw new ArgumentException();
                //PhpException.Throw(PhpError.Warning, LibResources.GetString("must_be_positive", "Length"));
                //return PhpValue.False;
            }

            return PhpValue.Create(string.CompareOrdinal(str1, 0, str2, 0, length));
        }

        /// <summary>
        /// Compares parts of two specified strings, honoring their case, using culture invariant comparison.
        /// </summary>
        /// <param name="str1">A string.</param>
        /// <param name="str2">A string.</param>
        /// <param name="length">The upper limit of the length of parts to be compared.</param>
        /// <returns>Returns -1 if <paramref name="str1"/> is less than <paramref name="str2"/>; +1 if <paramref name="str1"/> is greater than <paramref name="str2"/>,
        /// and 0 if they are equal.</returns>
        public static PhpValue strncasecmp(string str1, string str2, int length)
        {
            if (length < 0)
            {
                throw new ArgumentException();
                //PhpException.Throw(PhpError.Warning, LibResources.GetString("must_be_positive", "Length"));
                //return PhpValue.False;
            }

            length = Math.Max(Math.Max(length, str1.Length), str2.Length);

            return PhpValue.Create(System.Globalization.CultureInfo.InvariantCulture.CompareInfo
                .Compare(str1, 0, length, str2, 0, length, System.Globalization.CompareOptions.OrdinalIgnoreCase));
        }

        #endregion

        #region strpos, strrpos, stripos, strripos

        /// <summary>
        /// Retrieves the index of the first occurrence of the <paramref name="needle"/> in the <paramref name="haystack"/>.
        /// The search starts at the specified character position.
        /// </summary>
        /// <param name="haystack">The string to search in.</param>
        /// <param name="needle">
        /// The string or the ordinal value of character to search for. 
        /// If non-string is passed as a needle then it is converted to an integer (modulo 256) and the character
        /// with such ordinal value (relatively to the current encoding set in the configuration) is searched.</param>
        /// <param name="offset">
        /// The position where to start searching. Should be between 0 and a length of the <paramref name="haystack"/> including.
        /// </param>
        /// <returns>Non-negative integer on success, -1 otherwise.</returns>
        /// <exception cref="PhpException"><paramref name="offset"/> is out of bounds or <paramref name="needle"/> is empty string.</exception>
        [return: CastToFalse]
        public static int strpos(string haystack, PhpValue needle, int offset = 0)
        {
            return Strpos(haystack, needle, offset, StringComparison.Ordinal);
        }

        /// <summary>
        /// Retrieves the index of the first occurrence of the <paramref name="needle"/> in the <paramref name="haystack"/>
        /// (case insensitive).
        /// </summary>
        /// <remarks>See <see cref="Strpos(string,object,int)"/> for details.</remarks>
        /// <exception cref="PhpException">Thrown if <paramref name="offset"/> is out of bounds or <paramref name="needle"/> is empty string.</exception>
        [return: CastToFalse]
        public static int stripos(string haystack, PhpValue needle, int offset = 0)
        {
            return Strpos(haystack, needle, offset, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Retrieves the index of the last occurrence of the <paramref name="needle"/> in the <paramref name="haystack"/>.
        /// The search starts at the specified character position.
        /// </summary>
        /// <param name="haystack">The string to search in.</param>
        /// <param name="needle">The string or the ordinal value of character to search for. 
        /// If non-string is passed as a needle then it is converted to an integer (modulo 256) and the character
        /// with such ordinal value (relatively to the current encoding set in the configuration) is searched.</param>
        /// <param name="offset">
        /// The position where to start searching (is non-negative) or a negative number of characters
        /// prior the end where to stop searching (if negative).
        /// </param>
        /// <returns>Non-negative integer on success, -1 otherwise.</returns>
        /// <exception cref="PhpException">Thrown if <paramref name="offset"/> is out of bounds or <paramref name="needle"/> is empty string.</exception>
        [return: CastToFalse]
        public static int strrpos(string haystack, PhpValue needle, int offset = 0)
        {
            return Strrpos(haystack, needle, offset, StringComparison.Ordinal);
        }


        /// <summary>
        /// Retrieves the index of the last occurrence of the <paramref name="needle"/> in the <paramref name="haystack"/>
        /// (case insensitive).
        /// </summary>
        /// <remarks>See <see cref="Strrpos(string,object,int)"/> for details.</remarks>
        /// <exception cref="PhpException">Thrown if <paramref name="offset"/> is out of bounds or <paramref name="needle"/> is empty string.</exception>
        [return: CastToFalse]
        public static int strripos(string haystack, PhpValue needle, int offset = 0)
        {
            return Strrpos(haystack, needle, offset, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Implementation of <c>str[i]pos</c> functions.
        /// </summary>
        static int Strpos(string haystack, PhpValue needle, int offset, StringComparison comparisonType)
        {
            if (String.IsNullOrEmpty(haystack)) return -1;

            if (offset < 0 || offset >= haystack.Length)
            {
                if (offset != haystack.Length)
                {
                    throw new ArgumentOutOfRangeException();
                    //PhpException.InvalidArgument("offset", LibResources.GetString("arg_out_of_bounds"));
                }
                return -1;
            }

            var str_needle = PhpVariable.StringOrNull(needle);
            if (str_needle != null)
            {
                if (str_needle == String.Empty)
                {
                    throw new ArgumentException();
                    //PhpException.InvalidArgument("needle", LibResources.GetString("arg_empty"));
                    //return -1;
                }

                return haystack.IndexOf(str_needle, offset, comparisonType);
            }
            else
            {
                return haystack.IndexOf(chr_unicode((int)(needle.ToLong() % 256)), offset, comparisonType);
            }
        }

        /// <summary>
        /// Implementation of <c>strr[i]pos</c> functions.
        /// </summary>
        static int Strrpos(string haystack, PhpValue needle, int offset, StringComparison comparisonType)
        {
            if (String.IsNullOrEmpty(haystack)) return -1;

            int end = haystack.Length - 1;
            if (offset > end || offset < -end - 1)
            {
                throw new ArgumentOutOfRangeException();
                //PhpException.InvalidArgument("offset", LibResources.GetString("arg_out_of_bounds"));
                //return -1;
            }

            var str_needle = PhpVariable.StringOrNull(needle);
            if (offset < 0)
            {
                end += offset + (str_needle != null ? str_needle.Length : 1);
                offset = 0;
            }

            if (str_needle != null)
            {
                if (str_needle.Length == 0)
                {
                    throw new ArgumentException();
                    //PhpException.InvalidArgument("needle", LibResources.GetString("arg_empty"));
                    //return -1;
                }

                return haystack.LastIndexOf(str_needle, end, end - offset + 1, comparisonType);
            }
            else
            {
                return haystack.LastIndexOf(chr_unicode((int)(needle.ToLong() % 256)), end, end - offset + 1, comparisonType);
            }
        }

        #endregion

        #region strstr, stristr, strchr, strrchr

        #region Stubs

        /// <summary>
        /// Finds first occurrence of a string.
        /// </summary>
        /// <param name="haystack">The string to search in.</param>
        /// <param name="needle">The substring to search for.</param>
        /// <param name="beforeNeedle">If TRUE, strstr() returns the part of the haystack before the first occurrence of the needle. </param>
        /// <returns>Part of <paramref name="haystack"/> string from the first occurrence of <paramref name="needle"/> to the end 
        /// of <paramref name="haystack"/> or null if <paramref name="needle"/> is not found.</returns>
        /// <exception cref="PhpException">Thrown when <paramref name="needle"/> is empty.</exception>
        [return: CastToFalse]
        public static string strstr(string haystack, PhpValue needle, bool beforeNeedle = false)
        {
            return StrstrImpl(haystack, needle, StringComparison.Ordinal, beforeNeedle);
        }

        /// <summary>
        /// Finds first occurrence of a string. Alias of <see cref="strstr(string,PhpValue,bool)"/>.
        /// </summary>
        /// <remarks>See <see cref="Strstr(string,object)"/> for details.</remarks>
        /// <exception cref="PhpException">Thrown when <paramref name="needle"/> is empty.</exception>
        public static string strchr(string haystack, PhpValue needle) => strstr(haystack, needle);

        /// <summary>
        /// Case insensitive version of <see cref="Strstr(string,object)"/>.
        /// </summary>
        /// <param name="haystack"></param>
        /// <param name="needle"></param>
        /// <param name="beforeNeedle">If TRUE, strstr() returns the part of the haystack before the first occurrence of the needle. </param>
        /// <exception cref="PhpException">Thrown when <paramref name="needle"/> is empty.</exception>
        [return: CastToFalse]
        public static string stristr(string haystack, PhpValue needle, bool beforeNeedle = false)
        {
            return StrstrImpl(haystack, needle, StringComparison.OrdinalIgnoreCase, beforeNeedle);
        }

        #endregion

        /// <summary>
        /// This function returns the portion of haystack  which starts at the last occurrence of needle  and goes until the end of haystack . 
        /// </summary>
        /// <param name="haystack">The string to search in.</param>
        /// <param name="needle">
        /// If needle contains more than one character, only the first is used. This behavior is different from that of strstr().
        /// If needle is not a string, it is converted to an integer and applied as the ordinal value of a character.
        /// </param>
        /// <returns>This function returns the portion of string, or FALSE  if needle  is not found.</returns>
        /// <exception cref="PhpException">Thrown when <paramref name="needle"/> is empty.</exception>
        [return: CastToFalse]
        public static string strrchr(string haystack, PhpValue needle)
        {
            if (haystack == null)
                return null;

            char charToFind;
            string str_needle;

            if ((str_needle = PhpVariable.AsString(needle)) != null)
            {
                if (str_needle.Length == 0)
                {
                    throw new ArgumentException();
                    //PhpException.InvalidArgument("needle", LibResources.GetString("arg_empty"));
                    //return null;
                }

                charToFind = str_needle[0];
            }
            else
            {
                charToFind = chr_unicode((int)(needle.ToLong() % 256))[0];
            }

            int index = haystack.LastIndexOf(charToFind);
            if (index < 0)
                return null;

            return haystack.Substring(index);
        }

        /// <summary>
        /// Implementation of <c>str[i]{chr|str}</c> functions.
        /// </summary>
        internal static string StrstrImpl(string haystack, PhpValue needle, StringComparison comparisonType, bool beforeNeedle)
        {
            if (haystack == null) return null;

            int index;
            var str_needle = PhpVariable.StringOrNull(needle);
            if (str_needle != null)
            {
                if (str_needle == String.Empty)
                {
                    throw new ArgumentException();
                    //PhpException.InvalidArgument("needle", LibResources.GetString("arg_empty"));
                    //return null;
                }

                index = haystack.IndexOf(str_needle, comparisonType);
            }
            else
            {
                if (comparisonType == StringComparison.Ordinal)
                {
                    index = haystack.IndexOf((char)(needle.ToLong() % 256));
                }
                else
                {
                    index = haystack.IndexOf(chr_unicode((int)(needle.ToLong() % 256)), comparisonType);
                }
            }

            return (index == -1) ? null : (beforeNeedle ? haystack.Substring(0, index) : haystack.Substring(index));
        }

        #endregion

        #region strpbrk

        /// <summary>
        /// Finds first occurence of any of given characters.
        /// </summary>
        /// <param name="haystack">The string to search in.</param>
        /// <param name="charList">The characters to search for given as a string.</param>
        /// <returns>Part of <paramref name="haystack"/> string from the first occurrence of any of characters contained
        /// in <paramref name="charList"/> to the end of <paramref name="haystack"/> or <B>null</B> if no character is
        /// found.</returns>
        /// <exception cref="PhpException">Thrown when <paramref name="charList"/> is empty.</exception>
        [return: CastToFalse]
        public static string strpbrk(string haystack, string charList)
        {
            if (charList == null)
            {
                throw new ArgumentException();
                //PhpException.InvalidArgument("charList", LibResources.GetString("arg_empty"));
                //return null;
            }

            if (haystack == null) return null;

            int index = haystack.IndexOfAny(charList.ToCharArray());
            return (index >= 0 ? haystack.Substring(index) : null);
        }

        #endregion

        #region strtolower, strtoupper, strlen

        /// <summary>
        /// Returns string with all alphabetic characters converted to lowercase. 
        /// Note that 'alphabetic' is determined by the current culture.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The lowercased string or empty string if <paramref name="str"/> is null.</returns>
        public static string strtolower(string str) => str.ToLowerInvariant();
        //{
        //    // TODO: Locale: return (str == null) ? string.Empty : str.ToLower(Locale.GetCulture(Locale.Category.CType));
        //}

        /// <summary>
        /// Returns string with all alphabetic characters converted to lowercase. 
        /// Note that 'alphabetic' is determined by the current culture.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The lowercased string or empty string if <paramref name="str"/> is null.</returns>
        public static string strtoupper(string str) => str.ToUpperInvariant();
        //{
        //    // TODO: Locale: return (str == null) ? string.Empty : str.ToUpper(Locale.GetCulture(Locale.Category.CType));
        //}

        /// <summary>
        /// Returns the length of a string.
        /// </summary>
        public static int strlen(string x) => (x != null) ? x.Length : 0;

        /// <summary>
        /// Returns the length of a string.
        /// </summary>
        public static int strlen(PhpString x) => x.Length;

        #endregion
    }
}
