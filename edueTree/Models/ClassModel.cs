using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace edueTree.Models
{
    public class ClassModel
    {
        private dbContainer db = new dbContainer();
        // public IEnumerable<SelectListItem> SubjetList { get; set; }


        public ClassModel()
        {
            SubLists = db.Subjects.Select(a => new Subjectviewmodel
            {
                Subid = a.subjectId,
                Subname = a.subjectName
            }).ToList();

            DivisionLists = new[]
        {
            new Subjectviewmodel { Subname = "A" },
            new Subjectviewmodel { Subname = "B" },
            new Subjectviewmodel { Subname = "C"},
            new Subjectviewmodel { Subname = "D" },
            new Subjectviewmodel { Subname = "E" },
            new Subjectviewmodel { Subname = "F" },
            new Subjectviewmodel { Subname = "G"},
            new Subjectviewmodel { Subname = "H" },
            new Subjectviewmodel { Subname = "I" },
            new Subjectviewmodel { Subname = "J" },
            new Subjectviewmodel { Subname = "K"},
            new Subjectviewmodel { Subname = "L" },
        }.ToList();
        }

        public IList<Subjectviewmodel> SubLists { get; set; }
        public IList<Subjectviewmodel> DivisionLists { get; set; }
       // public IEnumerable<SelectListItem> DivisionLists { get; set; }


        public string ClassName { get; set; }

        
        public string Division { get; set; }
        public string Subjects { get; set; }

        public int ClassId { get; set; }

        public Boolean A { get; set; }
        public Boolean B { get; set; }
        public Boolean C { get; set; }
        public Boolean D { get; set; }
        public Boolean E { get; set; }
        public Boolean F { get; set; }
        public Boolean G { get; set; }
        public Boolean H { get; set; }
        public Boolean I { get; set; }
        public Boolean J { get; set; }
        public Boolean K { get; set; }
        public Boolean L { get; set; }

        public String SubjectId { get; set; }
        public String SubjectName { get; set; }

        //public String SubjectList { get; set; }
    }

    public class Subjectviewmodel
    {
        public int Subid { get; set; }
        public string Subname { get; set; }
        public bool Checked { get; set; }
    }

    public partial class ClassDivision
    {
        public IEnumerable<SelectListItem> ClassList { get; set; }
       // public IEnumerable<SelectListItem> DivisionList { get; set; }
    }

    public partial class Holiday
    {
        public IEnumerable<SelectListItem> YearidList { get; set; }
    }

   
}