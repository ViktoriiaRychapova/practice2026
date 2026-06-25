using System.Linq;

namespace task01;

public static class StringExtensions
{
 public static bool IsPalindrome(this string input)
 {
 if (string.IsNullOrWhiteSpace(input))
 return false;

 var normalized = new string(
 input
 .ToLowerInvariant()
 .Where(c => !char.IsWhiteSpace(c) && !char.IsPunctuation(c))
 .ToArray()
 );

 var reversed = new string(normalized.Reverse().ToArray());

 return normalized == reversed;
 }
}
