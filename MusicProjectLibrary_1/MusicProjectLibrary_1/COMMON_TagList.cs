//========================================================================
// Name:     COMMON_TagList.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  A List<> of tags.  Stores tags and provides methods to add,
//           query and remove them.
// Comments: 
//========================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace JAudioTags
{
    /// <summary>
    /// When an audio file is read from disc, a list of tags is created.
    /// As we modify tags, it is this list that is changed.
    /// this class implements a list of tags.
    /// </summary>
    internal class TagList
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "TagList:                   1.02";


        /// <summary>
        /// What kind of audio file are we working with?
        /// </summary>
        public AudioFileTypes AudioFileType { get; protected set; }


        /// <summary>
        /// The actual list of tags
        /// </summary>
        private List<TagType> TheList;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AudioType">What kind of audio file will this 
        /// be used with?</param>
        public TagList(AudioFileTypes AudioType)
        {
            AudioFileType = AudioType;
            TheList = new List<TagType>();
        }


        /// <summary>
        /// Builds a new tag and adds it to the tag list.
        /// </summary>
        /// <param name="Name">Name field of the new tag</param>
        /// <param name="Value">Value field of the new tag</param>
        public void AddTag(string Name, string Value) => TheList.Add(new TagType(Name, Value));


        /// <summary>
        /// Adds a tag to the tag list.
        /// </summary>
        /// <param name="NewTag">The tag to be added</param>
        public void AddTag(TagType NewTag) => TheList.Add(NewTag);


        /// <summary>
        /// Returns how many tags currently in the list.
        /// </summary>
        /// <returns>How many tags</returns>
        public int CountTags() => TheList.Count;


        /// <summary>
        /// Counts how many tags have the specified name.
        /// </summary>
        /// <param name="Name">The name to be searched for.</param>
        /// <returns>How many are found</returns>
        public int CountTags(string Name)
            => TheList.Where(n => n.Name.JToUpper() == Name.JToUpper()).Count();


        /// <summary>
        /// Sees whether any tags with the given name exist.
        /// </summary>
        /// <param name="Name">The name to be searched for.</param>
        /// <returns>Whether or not any are found</returns>
        public bool Exists(string Name)
        {
            if (CountTags(Name) > 0)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Returns the value field of the first tag in the list 
        /// having the name as specified.
        /// </summary>
        /// <param name="Name">The name to be searched for.</param>
        /// <returns>The value of the first matching tag</returns>
        public string First(string Name)
        {
            if (Exists(Name))
            {
                string V = TheList.Where(t => t.Name.JToUpper() == Name.JToUpper()).ToList().First().Value;
                return V;
            }
            else
                return "";
        }


        /// <summary>
        /// Removes all tags from the list which have name equal
        /// to the value specified.
        /// </summary>
        /// <param name="Name">The name to be searched for.</param>
        public void RemoveAll(string Name)
            => TheList.RemoveAll(n => n.Name.JToUpper() == Name.JToUpper());


        /// <summary>
        /// Removes the first tag from the list which has name
        /// equal to the value specified.
        /// </summary>
        /// <param name="Name">The name to be searched for.</param>
        public void RemoveFirst(string Name)
        {
            var Matches = TheList.Where(n => n.Name.JToUpper() == Name.JToUpper());
            if (Matches.Count() > 0)
                TheList.Remove(Matches.First());
        }


        /// <summary>
        /// Removes any tags with name and value matching those specified
        /// </summary>
        /// <param name="Name">Name to match</param>
        /// <param name="Value">Value to match</param>
        public void RemoveExact(string Name, string Value)
        {
            var Target = from Tag in TheList
                         where (Tag.Name.JToUpper() == Name.JToUpper()
                                && Tag.Value.JToUpper() == Value.JToUpper())
                         select Tag;
            if (Target.Count() > 0)
                TheList.Remove(Target.First());
        }


        /// <summary>
        /// Remove all tags with the same name and adds a new tag
        /// with the name and value supplied
        /// </summary>
        /// <param name="Name">Name to be removed then added</param>
        /// <param name="Value">Value for the new tag to be added</param>
        public void ReplaceAll(string Name, string Value)
        {
            RemoveAll(Name);
            AddTag(Name, Value);
        }


        /// <summary>
        /// Returns a class object as a string
        /// </summary>
        /// <returns>The string</returns>
        public override string ToString()
        {
            string Temp = "";
            foreach (var Tag in TheList)
                Temp += Tag + "\n";
            return Temp;
        }


        /// <summary>
        /// Makes this class enumerable by publishing the 
        /// enumerator of the List member
        /// </summary>
        /// <returns>An enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return TheList.GetEnumerator();
        }


        /// <summary>
        /// A debugging method.
        /// Adds some FLAC style tags to be used for testing.
        /// </summary>
        public void AddFLACTestTags()
        {
            AddTag("ARTIST", "Four Tops, The");
            AddTag("ALBUMARTIST", "Various - Motown");
            AddTag("ALBUM", "Chartbusters Volume 3");
            AddTag("COMPOSER", "Berry Gordy");
            AddTag("DISCNUMBER", "1/1");
            AddTag("TRACK", "03");
            AddTag("GENRE", "Soul");
            AddTag("COMMENT", "What a cracker!");
            AddTag("YEAR", "1966");
            AddTag("TITLE", "Still Waters (Run Deep)");
        }


        /// <summary>
        /// A debugging method.
        /// Adds ID3 v2.3 style tags to used for testing.
        /// </summary>
        public void AddV23TestTags()
        {
            AddTag("TPE1", "Four Tops, The");
            AddTag("TPE2", "Various - Motown");
            AddTag("TALB", "Chartbusters Volume 3");
            AddTag("TCOM", "Berry Gordy");
            AddTag("TPOS", "1/1");
            AddTag("TRCK", "03");
            AddTag("TCON", "Soul");
            AddTag("COMM", "What a cracker!");
            AddTag("TYER", "1966");
            AddTag("TIT2", "Still Waters (Run Deep)");
        }


        /// <summary>
        /// A debugging method.  Lets you manipulate a list 'by hand'.
        /// </summary>
        public void ModifyList()
        {
            int a;
            string Name, Value;
            do
            {
                Console.WriteLine("  1. Add a tag");
                Console.WriteLine("  2. Count tags");
                Console.WriteLine("  3. See if tag exists");
                Console.WriteLine("  4. View first");
                Console.WriteLine("  5. RemoveAll");
                Console.WriteLine("  6. RemoveFirst");
                Console.WriteLine("  7. RemoveExact");
                Console.WriteLine("  8. ReplaceAll");
                Console.WriteLine("  9. Exit");
                a = Helpers.readInt("Enter choice: ", 1, 9);
                switch (a)
                {
                    case 1:
                        Name = Helpers.readString("Enter tag name:  ");
                        Value = Helpers.readString("Enter tag value: ");
                        AddTag(Name, Value);
                        break;
                    case 2:
                        Name = Helpers.readString("Enter tag name:  ");
                        Console.WriteLine("Tag exists " + CountTags(Name) + " times.");
                        break;
                    case 3:
                        Name = Helpers.readString("Enter tag name:  ");
                        if (Exists(Name))
                            Console.WriteLine(Name + " exists.");
                        else
                            Console.WriteLine(Name + " does not exist.");
                        break;
                    case 4:
                        Name = Helpers.readString("Enter tag name:  ");
                        Console.WriteLine("Value of first matching tag is: " + First(Name));
                        break;
                    case 5:
                        Name = Helpers.readString("Enter tag name:  ");
                        RemoveAll(Name);
                        break;
                    case 6:
                        Name = Helpers.readString("Enter tag name:  ");
                        RemoveFirst(Name);
                        break;
                    case 7:
                        Name = Helpers.readString("Enter tag name:  ");
                        Value = Helpers.readString("Enter tag value: ");
                        RemoveExact(Name, Value);
                        break;
                    case 8:
                        Name = Helpers.readString("Enter tag name:  ");
                        Value = Helpers.readString("Enter tag value: ");
                        ReplaceAll(Name, Value);
                        break;
                    default:
                        break;
                }
                Console.WriteLine("\n" + this + "\n");
            } while (a != 9);
        }
    }
}