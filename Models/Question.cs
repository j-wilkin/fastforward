using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace fastforward.Models
{
    public class Question
    {
        [Required]
        public string QuestionText { get; set; }
        public string Label { get; set; }
        [Required]
        public virtual List<Answer> Answers { get; set; }
        [Key]
        public int QuestionId { get; set; }
        public int Result { get; set; }
        public bool IsViewable { get; set; }
        [ForeignKey("Survey")]
        public int SurveyId { get; set; }

        public virtual Survey Survey { get; set; }
        public virtual int GroupIdA { get; set; }
        public virtual int GroupIdB { get; set; }
        public List<Checkbox> RadioButtonList { get; set; }

        public Question()
        {
        }

        public Question(string question, string label, List<Answer> answers, int result)
        {
            QuestionText = question;
            Label = label;
            Answers = answers;
            Result = result;

        }

        public void UpdateButtonList()
        {
            var radioButtons = new List<Checkbox>();
            foreach (var answer in Answers)
            {
                radioButtons.Add(new Checkbox(answer.Text, false));
            }

        }
    }
}
