using System.Text.RegularExpressions;
using UnityEngine;

namespace Framework.Core
{
	public static class StringExtensions
	{
		public static string SplitCamelCase(this string text)
		{
			return Regex.Replace( 
				Regex.Replace( 
					text, 
					@"(\P{Ll})(\P{Ll}\p{Ll})", 
					"$1 $2" 
				), 
				@"(\p{Ll})(\P{Ll})", 
				"$1 $2" 
			);
		}
	    
		public static string UppercaseFirst(this string text){
			if (string.IsNullOrEmpty(text))
				return string.Empty;
		    
			text = text.ToLower();
			char[] a = text.ToCharArray();
			a[0] = char.ToUpper(a[0]);
			return new string(a);
		}
		
		public static string ToTimeString(this long time, bool fullText = false)
		{
			var secondsText = fullText ? " Seconds" : "s";
			var minutesText = fullText ? " Minutes" : "m";
			var hoursText = fullText ? " Hours" : "h";
			var daysText = fullText ? " Days" : "d";
	        
			if (time < 60) // Seconds
			{
				return $"{time}{secondsText}";
			}

			if (time < 3600) // Minutes Seconds
			{
				return $"{time / 60}{minutesText} {time % 60}{secondsText}";
			}

			if (time < 86400) // Hours Minutes
			{
				return $"{time / 3600}{hoursText} {time / 60 % 60}{minutesText}";
			}

			// Days

			var days = time / 86400;
			return $"{days}{daysText} {(time - (days * 86400)) / 3600}{hoursText}";
		}
		
		public static string ToTimerString(this float time)
		{
			return $"{time:F0}s";
		}
		
		public static string ToFormattedString(this int amount)
		{
			float nNum = amount;
            
			if (nNum >= 1000000000000)
				return (nNum / 1000000000000).ToString("F2").Replace(',', '.') + "<color=yellow><size=75%>T</size></color>";
			else if (nNum >= 1000000000)
				return (nNum / 1000000000).ToString("F2").Replace(',', '.') + "<color=yellow><size=75%>B</size></color>";
			else if (nNum >= 10000000)
				return (nNum / 1000000).ToString("F2").Replace(',', '.') + "<color=yellow><size=75%>M</size></color>";
			else if (nNum >= 1000000)
				return (nNum / 1000000).ToString("F2").Replace(',', '.') + "<color=yellow><size=75%>M</size></color>";
			else if (nNum >= 100000)
				return (nNum / 10000).ToString("F2").Replace(',', '.') + "<color=yellow><size=75%>K</size></color>";
			else if (nNum >= 1000)
				return (nNum / 1000).ToString("F2").Replace(',', '.') + "<color=yellow><size=75%>K</size></color>";

			return amount.ToString();
		}
		
		public static string ToColor(this string text, Color color)
		{
			return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>";
		}
	}   
}

