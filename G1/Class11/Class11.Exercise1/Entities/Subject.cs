﻿namespace Class11.Exercise1.Entities
{
    public class Subject : BaseEntity
    {
        public string Title { get; set; }
        public Academy Academy { get; set; }
        public int Classes { get; set; }

        public Subject()
        { }
        public Subject(string title, Academy academy, int classes)
        {
            Title = title;
            Academy = academy;
            Classes = classes;
        }

        public override string Info()
        {
            return $"{Title} from the {Academy} lasts for {Classes} classes.";
        }
    }

    public enum Academy
    {
        Design,
        Programming,
        Testing
    }
}
