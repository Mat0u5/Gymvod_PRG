using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassPlayground
{
    internal class Student
    {
        public int year;
        public long id;
        public string name;
        public Dictionary<String, List<double[]>> subjects;
        public Student(string name, int year)
        {
            this.name = name;
            this.year = year;

            Random rnd = new Random();
            string idStr = Convert.ToString(rnd.Next(100000)) + Convert.ToString(rnd.Next(1000000));
            id = long.Parse(idStr);
            subjects = new Dictionary<String, List<double[]>>();
        }
        public void AddSubject(String subjectName)
        {
            List<double[]> marks = new List<double[]>();
            if (subjects.ContainsKey(subjectName))
            {
                Console.WriteLine("This subject already exists");
                return;
            }
            subjects[subjectName] = marks;
        }
        public void AddGrade(String subjectName, double mark, double weight)
        {

            List<double[]> marks = subjects[subjectName];
            marks.Add(new double[] { mark,weight});
            subjects[subjectName] = marks;
        }
        public double CalculateSubjectGrade(String subjectName)
        {
            List<double[]> marks = subjects[subjectName];
            double count = 0;
            double sum = 0;
            foreach(double[] mark in marks)
            {
                sum += mark[0] * mark[1];
                count += mark[1];
            }
            if (count == 0)
            {
                Console.WriteLine("This subject does not have any grades");
                return 0;
            }
            return sum / count;
        }
        public double CaculateTotalGrade()
        {
            double count = 0;
            double sum = 0;
            foreach (List<double[]> marks in subjects.Values)
            {
                foreach (double[] mark in marks)
                {
                    sum += mark[0]* mark[1];
                    count += mark[1];
                }
            }
            if (count == 0)
            {
                Console.WriteLine("This student does not have any grades");
                return 0;
            }
            return sum / count;
        }

    }
}
