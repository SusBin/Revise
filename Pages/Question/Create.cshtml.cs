using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Revise.Models;
using System.Text.Json;

namespace Revise.Pages.Question
{
    public class CreateModel : PageModel
    {
        public RevisionQuestion NewQuestion { get; set; }

        public void OnPost()
        {
            var questions = new List<RevisionQuestion>();
            if (System.IO.File.Exists("questions.json"))
            {
                var json = System.IO.File.ReadAllText("questions.json");
                questions = JsonSerializer.Deserialize<List<RevisionQuestion>>(json);
            }

            // Auto-generate the Id field
            NewQuestion.Id = questions.Any() ? questions.Max(q => q.Id) + 1 : 1;

            questions.Add(NewQuestion);
            var newJson = JsonSerializer.Serialize(questions);
            System.IO.File.WriteAllText("questions.json", newJson);
        }
    }
}
