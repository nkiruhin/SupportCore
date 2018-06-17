using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.App.Classes
{
    public class ReadmineIssue
    {
            [Display(Name = "Номер заявки")]
            [DataType(DataType.Url)]
            public string Id { set; get; }
            [Display(Name ="Последний комментарий")]
            public string LastNote { set; get; }
            [Display(Name = "Автор комментария")]
            public string LastNoteUser { set; get; }
            [Display(Name = "Текущий статус")]
            public string Status { set; get; }
            [Display(Name = "Назначена")]
            public string AssignTo { set; get; }
            [Display(Name = "Дата комментария")]
            public DateTime DateNote { set; get; }
            [Display(Name = "Дата изменения")]
            public DateTime DateUpdate { set; get; }
            [Display(Name = "Тема")]
            [ScaffoldColumn(false)]
            public string Subject { set; get; }
            [ScaffoldColumn(false)]
            [Display(Name = "Описание")]
            public string Description { set; get; }
    }
    //public class NewRedmineIssue
    //{

    //}
}
