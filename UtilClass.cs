using Microsoft.Win32;
using Study_Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
//Emil Namaan Reuben Murray
// Student Number : 20117913
namespace POE_Task_1
{
    public class UtilClass
    {
        //constant value for 0
        private const int V = 0;
        //Method that writes the study sessions to a textfile
        public void writeToFile()
        {
            //---------CODE ATTRIBUTION---------
            //The following method was from Microsoft Docs
            //Authors : BillWagner, IEvangelist, nschonni, jdmartinez36, mairaw, Youssef1313, nemrism,yishengjin1413, nxtn, pkulikov, mjhoffman65, mikeblome, guardrex
            //Link: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-write-to-a-text-file
            //---------END---------
            //Takes the text from the output
            List<string> items = populateStudySessions();
            //creates a new saveFileDialog
            SaveFileDialog newsaveFileDialog = new SaveFileDialog();
            string print_Output = "";
            foreach (string item in items)
            {
                print_Output += item;
            }

            //Allows the user to select textfile
            newsaveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (newsaveFileDialog.ShowDialog() == true)
            {
                //Checks if the user selects the textfile option
                if (newsaveFileDialog.FilterIndex == 1)
                {
                    try
                    {
                        //Write to text file 
                        using (StreamWriter writer = new StreamWriter(newsaveFileDialog.FileName, false))
                        {
                            writer.WriteLine(print_Output);
                            MessageBox.Show("File Successfully saved", "Smart Study Planner", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                    }
                    catch
                    {
                        //Shows error message if any occur
                        MessageBox.Show("File not Saved", "Smart Study Planner", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        //Method which allows the user to print the report
        public void PrintStudySessions()
        {
            try
            {
                //---------CODE ATTRIBUTION---------
                //The following method was from Someblokeontheinternet.blogspot.com
                //Authors : Unknown
                //Link: http://someblokeontheinternet.blogspot.com/2009/11/printing-contents-of-text-box-or-just.html (Not a secure website)

                //Creates a print dialog which allows the user to print
                PrintDialog printDialog = new PrintDialog();
                //displays the printing option
                if (printDialog.ShowDialog().GetValueOrDefault())
                {
                    List<string> studyItems = populateStudySessions();
                    //flowdocument used to state each line of the textbox
                    FlowDocument flowDocument = new FlowDocument();
                    Paragraph newParagraph = new Paragraph();

                    newParagraph.Margin = new Thickness(15);
                    newParagraph.Inlines.Add(new Run("Student Name : " + WorkerLists.studentList.Student_Name + " " + WorkerLists.studentList.Student_Surname + "\nStudent Number : " + WorkerLists.studentList.Student_Number));
                    flowDocument.Blocks.Add(newParagraph);

                    //Loop going throigh the textfile, adding margins, paragrapghs and spaces to mimic the report
                    foreach (string line in studyItems)
                    {
                        Paragraph newPara = new Paragraph();
                        newPara.Margin = new Thickness(15);
                        newPara.Inlines.Add(new Run(line));
                        flowDocument.Blocks.Add(newPara);
                    }
                    //---------END---------
                    //Prints the report caling documentpaginator  sending the flowducument to print
                    DocumentPaginator newDocPag = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;
                    //sends object to print
                    printDialog.PrintDocument(newDocPag, "Study Sessions recorded for : " + WorkerLists.studentList.Student_Name + " " + WorkerLists.studentList.Student_Surname);
                }
            }
            catch
            {
                MessageBox.Show("An unexpected error occurred", "Smart Study Planner", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static Tuple<string> findSelectedModule(string selectedModuleCode)
        {
            //Creates object of module and stores the first module with the same selected module code
            Module newModule = WorkerLists.modulesList.Where(x => x.Module_code.Equals(selectedModuleCode)).First();
            Tuple<string> newTuple = new Tuple<string>(newModule.Module_code);
            return newTuple;
        }
        public void deleteModule(int selectedIndex, string selectedModuleCode)
        {
            try
            {
                //checks if the selected index is selected
                if (selectedIndex != -1)
                {
                    //Conformation message
                    if (MessageBox.Show("Are you sure you want to delete this module?", "Notice", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
                    {
                        //Cancel message
                        MessageBox.Show("Module delete cancelled !", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {//Linq which deletes the module and studying hours related to the module, selected by the user
                        WorkerLists.modulesList.RemoveAll(x => x.Module_code == selectedModuleCode);
                        WorkerLists.studyingList.RemoveAll(x => x.Module_Studied == selectedModuleCode);
                        //Confirms that the items were deleted
                        if (WorkerLists.modulesList.RemoveAll(x => x.Module_code == selectedModuleCode) > 0 && WorkerLists.studyingList.RemoveAll(x => x.Module_Studied == selectedModuleCode) > 0)
                        {
                            MessageBox.Show("Module not found.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Module deleted.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }

                else
                {
                    MessageBox.Show("Please select a module.", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurred. Please try again.", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        public List<string> populateModuleCode()
        {
            List<string> module_Names = new List<string>();
            IEnumerable<Module> moduleList = from moduleList1 in WorkerLists.modulesList
                                             select moduleList1;
            moduleList.ToList();
            foreach (Module m in moduleList)
            {
                module_Names.Add(m.Module_code);
            }

            return module_Names;
        }
        public List<string> populateStudySessions()
        {
            //Creates list
            List<string> sessions = new List<string>();
            //Linq to get all the study sessions recorded
            IEnumerable<Studying> StudySession = from studyingList in WorkerLists.studyingList
                                                 select studyingList;
            StudySession.ToList();
            string result = "";
            //Loops through the list and outputs the result
            foreach (Studying m in StudySession)
            {
                result = "Module : " + m.Module_Studied + "\nDate studied : " + m.Study_Date + "\nNo. hours : " + m.Study_hours;
                sessions.Add(result);
            }

            return sessions;
        }
        public static Tuple<string, string, string, string, string, string> selectedModuleDisplay(string selectedModuleCode)
        {
            //Stores an object with the module that the user selected
            Module newModule = WorkerLists.modulesList.Where(x => x.Module_code.Equals(selectedModuleCode)).First();
            //Creates an object of the calcualtion class and performs the calculation on the chosen module
            CalculationClass newCalculation = new CalculationClass();
            double studyHours = newCalculation.calcSelfStudyHours(selectedModuleCode);

            //Linq for gettings the objects where the selected module is being studued
            IEnumerable<Studying> studyList = from studyList1 in WorkerLists.studyingList
                                              where studyList1.Module_Studied == selectedModuleCode
                                              select studyList1;
            studyList.ToList();
            double totalStudyHours = 0;
            double totalHoursForSemester = WorkerLists.semesterList.Num_Weeks * studyHours;
            foreach (Studying item in studyList)
            {
                //Gets the ttoal hours studied for this module
                totalStudyHours += item.Study_hours;
            }
            //Calculation which gets the number of hours from when the semester started to the current date of the semester in this instance of date and time
            double remainingSelfStudyPerWeek = Convert.ToInt32(Math.Floor(Convert.ToDecimal(Math.Abs((Convert.ToDateTime(WorkerLists.semesterList.Start_Date) - DateTime.Now).Days) / 7)) + 1) * studyHours;
            //
            studyHours = studyHours - totalStudyHours;
            //Gets the total hours after sutdying hours have been deducted
            double hours = remainingSelfStudyPerWeek - totalStudyHours;
            double hoursNotStudied = hours - Math.Abs(studyHours);
            //Calculates the total hours for the semester regardless of date.
            totalHoursForSemester = totalHoursForSemester - totalStudyHours;

            if (hours <= V)
            {
                hours = V;

            }
            //Checks if study hours for week is less than 0 so that it can deduct from the previous weeks
            if (studyHours <= V)
            {
                studyHours = V;
                hoursNotStudied = hours;

            }
            if (totalHoursForSemester <= V)
            {
                totalHoursForSemester = V;
            }


            //Creates a tuple to return, storing the items that need to be send
            Tuple<string, string, string, string, string, string> newTuple = new Tuple<string, string, string, string, string, string>
            (newModule.Module_code, "Module Name : " + newModule.Module_Name, "Remaining self-study hours till current date : " + hoursMinutesOutput(hours), "No. hours of self study for this week : " + hoursMinutesOutput(studyHours), "No. hours from previous weeks : " + hoursMinutesOutput(hoursNotStudied), "No. hours remaining for entire semester : " + hoursMinutesOutput(totalHoursForSemester));

            return newTuple;
        }
        //Output for formating the hours into hours and minutes
        private static string hoursMinutesOutput(double value)
        {
            string output = Decimal.ToInt32((decimal)value) + " hrs " + Math.Floor(getDecimalPart(value)).ToString() + " mins";
            return output;
        }
        //Method to convert the decimal part of a fraction into minutes
        private static double getDecimalPart(double studyHours)
        {
            double decimalNumber = (double)((decimal)studyHours % 1 * 100);
            decimalNumber = decimalNumber / 100 * 60;

            return decimalNumber;
        }
        public List<string> populateModuleHours()
        {
            List<string> module_Hours = new List<string>();

            foreach (Module m in WorkerLists.modulesList)
            {
                CalculationClass newCalculation = new CalculationClass();
                double studyHours = newCalculation.calcSelfStudyHours(m.Module_code);

                if (WorkerLists.semesterList != null)
                {
                    //list which stores the objects that mathes with the same module code
                    IEnumerable<Studying> studyList = from studyList1 in WorkerLists.studyingList
                                                      where studyList1.Module_Studied == m.Module_code
                                                      select studyList1;
                    studyList.ToList();
                    double totalStudyHours = 0;
                    foreach (Studying item in studyList)
                    {
                        //Gets the ttoal hours studied for this module
                        totalStudyHours += item.Study_hours;
                    }
                    //Calculation which gets the number of hours from when the semester started to the current date of the semester in this instance of date and time
                    double remainingSelfStudyPerWeek = Convert.ToInt32(Math.Floor(Convert.ToDecimal(Math.Abs((Convert.ToDateTime(WorkerLists.semesterList.Start_Date) - DateTime.Now).Days) / 7)) + 1) * studyHours;
                    //
                    studyHours = studyHours - totalStudyHours;
                    //Gets the total hours after sutdying hours have been deducted
                    double hours = remainingSelfStudyPerWeek - totalStudyHours;

                    //Converts the decial part of the fraction to minutes
                    double decimalHours = (double)((decimal)hours % 1 * 100);
                    decimalHours = decimalHours / 100 * 60;
                    //displays the hours in a certain form
                    string remainingSelfHours = Decimal.ToInt32((decimal)hours) + " hrs " + Math.Floor(decimalHours).ToString() + " mins";
                    module_Hours.Add(remainingSelfHours);
                }
            }
            return module_Hours;
        }
    }
}
