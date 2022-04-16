using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Study_Library;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
//Emil Namaan Reuben Murray
//20117913
// Image reference : Max Pixel, 2021. Abstract Graphic Background Pattern Design. [image] Available at: <https://www.maxpixel.net/Abstract-Graphic-Background-Pattern-Design-2829965> [Accessed 15 September 2021].
//Icon picture : findicons.2020. Study App Icon. [Online]. Available at: https://findicons.com/icon/585329/study_app [Accessed on 14 September 2021]
namespace POE_Task_1
{
    public partial class MainWindow : Window
    {
        //Brush created globally such taht all methods have access to it
        BrushConverter bc = new BrushConverter();
        string selectedModuleCOde;
        Validation newVal = new Validation();
        public MainWindow()
        {
            InitializeComponent();
            //Sorts buttons and Grids to postion itself correctly on launch
            sortGridsOnLaunch();
            //Sorts the profile section in the side panel grid
            rectProfile.Visibility = Visibility.Hidden;
            lblStudentDisplayName.Visibility = Visibility.Hidden;
            lblStudentNumberD.Visibility = Visibility.Hidden;
        }
        //Method to sort grids and buttons on the start up of the application
        private void sortGridsOnLaunch()
        {
            btnHome1.Background = Brushes.White;
            btnHome1.Foreground = (Brush)bc.ConvertFrom("#FF1589FF");

            btnModules.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnModules.Foreground = Brushes.White;

            btnSemesterInfo.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnSemesterInfo.Foreground = Brushes.White;

            btnPersonalDetails.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnPersonalDetails.Foreground = Brushes.White;

            btnStudyHistory.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnStudyHistory.Foreground = Brushes.White;

            gridWelcome.Visibility = Visibility.Visible;
            gridModuleDisplay.Visibility = Visibility.Hidden;
            gridModuleInfo.Visibility = Visibility.Hidden;
            gridSemesterInfo.Visibility = Visibility.Hidden;
            gridPersonalDetails.Visibility = Visibility.Hidden;
            gridStudying.Visibility = Visibility.Hidden;
            gridStudySessionDisplay.Visibility = Visibility.Hidden;
        }
        //clears textboces and any error messages that may occur in the add module grid
        private void clearModuleForm()
        {
            txbClassHours.Clear();
            txbModuleName.Clear();
            txbModuleCode.Clear();
            txbNumCredits.Clear();

            lblClassErrors.Visibility = Visibility.Hidden;
            lblModuleNameError.Visibility = Visibility.Hidden;
            lblCreditsError.Visibility = Visibility.Hidden;
            lblModuleError.Visibility = Visibility.Hidden;
        }
        //Method which displays the modules and remaining study hours into 2 lists
        private void displayList()
        {
            lstModuleCode.Items.Clear();
            lstModuleHours.Items.Clear();

            UtilClass newUtil = new UtilClass();

            List<string> moduleList = newUtil.populateModuleCode();
            List<string> hoursList = newUtil.populateModuleHours();

            foreach (string item in moduleList)
            {
                lstModuleCode.Items.Add(item);
            }
            foreach (string item in hoursList)
            {
                lstModuleHours.Items.Add(item);
            }
        }
        //Method which displays the study information in a list
        private void dispalyStudysession()
        {
            lstStudySession.Items.Clear();

            UtilClass newUtil = new UtilClass();

            List<string> sessionList = newUtil.populateStudySessions();

            foreach (string item in sessionList)
            {
                lstStudySession.Items.Add(item);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //declare variables
            double numOfCredits;
            double classHours;
            string moduleCode;
            string moduleName;
            bool moduleAdded = false;
            try
            {
                //resets errors
                lblClassErrors.Visibility = Visibility.Hidden;
                lblModuleNameError.Visibility = Visibility.Hidden;
                lblCreditsError.Visibility = Visibility.Hidden;
                lblModuleError.Visibility = Visibility.Hidden;
                //Gets data from GUI components
                numOfCredits = Convert.ToDouble(txbNumCredits.Text);
                classHours = Convert.ToDouble(txbClassHours.Text);
                moduleCode = txbModuleCode.Text;
                moduleName = txbModuleName.Text;
                //Trims strings
                moduleCode.Trim();
                moduleName.Trim();
                //Loops to check if module is alrady added
                foreach (Module item in WorkerLists.modulesList)
                {
                    if (moduleCode == item.Module_code)
                    {
                        moduleAdded = true;
                    }
                }
                //Validation to add module
                CalculationClass newCalc = new CalculationClass();
                if (newCalc.onceofStudyHours(classHours, numOfCredits) > 0 && moduleAdded == false && newVal.checkStringInput(moduleCode) && newVal.checkStringInput(moduleName) && newVal.checkSpecial(moduleCode) == false
                    && newVal.checkSpecial(moduleName) == false && numOfCredits > 0 && classHours > 0 && numOfCredits < 1000 && classHours <= 168)
                {
                    //Creates object and adds it the list
                    Study_Library.Module newModule = new Module(moduleCode, moduleName, numOfCredits, classHours);
                    Study_Library.WorkerLists.modulesList.Add(newModule);
                    //Clears module Gui components and adds the updated module to the listbox
                    clearModuleForm();
                    displayList();
                    //Confirmation message
                    MessageBox.Show("Module : " + moduleCode + " Added", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    //alerts the user that the module is already created
                    if (moduleAdded)
                    {
                        MessageBox.Show("Module : " + moduleCode + " was already added.", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    if (newCalc.onceofStudyHours(classHours, numOfCredits) <= 0)
                    {
                        MessageBox.Show("Please enter correct values for credits and class hours, due to incorrect calculation result", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    if (!newVal.checkStringInput(moduleCode) && !newVal.checkStringInput(moduleName) && newVal.checkSpecial(moduleCode) && newVal.checkSpecial(moduleName) && numOfCredits < 0 && classHours < 0)
                    {
                        MessageBox.Show("Please enter the correct values for Module Details", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    //CLears componenets
                    clearModuleForm();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter the correct values for Module Details", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
                clearModuleForm();
            }
        }
        //Method to confirm that the user would like to confirm closing the application
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Close App Notice",
               MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                //Creates a delay of 1.4 seconds
                int delay = 1400;
                //---------CODE ATTRIBUTION---------
                //The following method was tafrom StackOverFlow
                //Author : Carlos Loth
                //Link: https://stackoverflow.com/questions/2902327/c-sharp-winforms-change-cursor-icon-of-mouse
                Mouse.OverrideCursor = Cursors.Wait;
                //---------END---------
                Task.Delay(delay).Wait();
                Environment.Exit(1);
            }
        }
        private void btnModules_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //cehecks if there is an object of semester in the list
                if (Study_Library.WorkerLists.semesterList != null)
                {
                    moduleInfoClicked();
                }
                else
                {
                    MessageBox.Show("Please complete the Semester information first.", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurred", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnHome1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Resets buttons and Grids when the home button is clicekd. It will either fisplay the welcome grid if there are objects in student list and semester list
                btnModules.Background = (Brush)bc.ConvertFrom("#FF1589FF");
                btnModules.Foreground = Brushes.White;

                btnHome1.Background = Brushes.White;
                btnHome1.Foreground = (Brush)bc.ConvertFrom("#FF1589FF");

                btnPersonalDetails.Background = (Brush)bc.ConvertFrom("#FF1589FF");
                btnPersonalDetails.Foreground = Brushes.White;

                btnSemesterInfo.Background = (Brush)bc.ConvertFrom("#FF1589FF");
                btnSemesterInfo.Foreground = Brushes.White;

                btnStudyHistory.Background = (Brush)bc.ConvertFrom("#FF1589FF");
                btnStudyHistory.Foreground = Brushes.White;
                //Dispalys the appropriate grid depending if modules are added to the list
                if (Study_Library.WorkerLists.semesterList == null || Study_Library.WorkerLists.studentList == null)
                {
                    gridWelcome.Visibility = Visibility.Visible;
                    gridModuleDisplay.Visibility = Visibility.Hidden;
                    gridModuleInfo.Visibility = Visibility.Hidden;
                    gridSemesterInfo.Visibility = Visibility.Hidden;
                    gridPersonalDetails.Visibility = Visibility.Hidden;
                    gridStudying.Visibility = Visibility.Hidden;
                    gridStudySessionDisplay.Visibility = Visibility.Hidden;
                }
                else
                {
                    moduleDisplayClicked();
                    gridWelcome.Visibility = Visibility.Collapsed;
                    gridModuleDisplay.Visibility = Visibility.Visible;
                    gridModuleInfo.Visibility = Visibility.Hidden;
                    gridSemesterInfo.Visibility = Visibility.Hidden;
                    gridPersonalDetails.Visibility = Visibility.Hidden;
                    gridStudying.Visibility = Visibility.Hidden;
                    gridStudySessionDisplay.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception)
            { }
        }
        private void btnNext2_Click(object sender, RoutedEventArgs e)
        {
            //gets data from components
            int numWeeks;
            DateTime semesterStart;
            try
            {
                //resets errors
                lblNumWeeksError.Visibility = Visibility.Hidden;
                lblStartDateError.Visibility = Visibility.Hidden;
                //gets data from GUI components
                numWeeks = Convert.ToInt32(txbNumWeeks.Text);

                //Checks if it meeets requirements to be added in to the list
                if (numWeeks > 0 && numWeeks < 52 && dateSemesterStart.SelectedDate.HasValue)
                {
                    semesterStart = dateSemesterStart.SelectedDate.Value;
                    //Creates semester object and adds it the list
                    Semester newSemester = new Semester(numWeeks, semesterStart);
                    WorkerLists.semesterList = newSemester;
                    moduleInfoClicked();
                    btnToModuleInfo.Visibility = Visibility.Hidden;

                    MessageBox.Show("Semester information updated.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Please enter the correct values for Semester Details", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter the correct values for Semester Details", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnToSemesterGrid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //gets data from GUI compoenents and trims the strings
                string studentNumber = txbStudentNumber.Text;
                studentNumber.Trim();
                string firstName = txbFirstName.Text;
                firstName.Trim();
                string surname = txbSurname.Text;
                surname.Trim();
                string uniName = txbUniversityName.Text;
                uniName.Trim();

                //Checks if the requirements are met to create a studnet object
                if (newVal.checkNumberOnly(firstName) == false &&
                    newVal.checkNumberOnly(surname) == false &&
                    newVal.checkNumberOnly(uniName) == false &&
                    newVal.checkSpecial(firstName) == false &&
                    newVal.checkSpecial(surname) == false &&
                    newVal.checkSpecial(uniName) == false &&
                    newVal.checkStringInput(firstName) &&
                    newVal.checkStringInput(surname) &&
                    newVal.checkStringInput(uniName) &&
                    newVal.isNumber(studentNumber))
                {
                    //dispalys semester, clears list and adds a new student object to the list
                    semesterInfoClicked();
                    Study_Library.WorkerLists.studentList = null;
                    Study_Library.studentDetails newStudent = new studentDetails(studentNumber, firstName, surname, uniName);
                    Study_Library.WorkerLists.studentList = newStudent;
                    //Confirmation message, resets GUI componenets and adds user name and studnet number to the profile section
                    MessageBox.Show("Student Information updated.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                    lblStudentDisplayName.Content = WorkerLists.studentList.Student_Name;
                    lblStudentDisplayName.Visibility = Visibility.Visible;
                    lblStudentNumberD.Content = WorkerLists.studentList.Student_Number;
                    lblStudentNumberD.Visibility = Visibility.Visible;
                    rectProfile.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Please enter the correct values for Personal Details", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter the correct values for Personal Details", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        //displays the semester grid
        private void btnSemesterInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //checks if there is an object of student in the list
                if (Study_Library.WorkerLists.studentList != null)
                {
                    semesterInfoClicked();
                }
                else
                {
                    MessageBox.Show("Please complete the Personal information first.", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurred", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        //Sorts the grids and buttons to show the personal details grid
        private void personalDetailsClicked()
        {
            btnPersonalDetails.Background = Brushes.White;
            btnPersonalDetails.Foreground = (Brush)bc.ConvertFrom("#FF1589FF");

            btnHome1.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnHome1.Foreground = Brushes.White;

            btnModules.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnModules.Foreground = Brushes.White;

            btnSemesterInfo.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnSemesterInfo.Foreground = Brushes.White;

            btnStudyHistory.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnStudyHistory.Foreground = Brushes.White;

            gridPersonalDetails.Visibility = Visibility.Visible;
            gridWelcome.Visibility = Visibility.Hidden;
            gridModuleDisplay.Visibility = Visibility.Hidden;
            gridModuleInfo.Visibility = Visibility.Hidden;
            gridSemesterInfo.Visibility = Visibility.Hidden;
            gridStudying.Visibility = Visibility.Hidden;
        }
        //Sorts the grids and buttons to show the semester details grid
        private void semesterInfoClicked()
        {
            btnSemesterInfo.Background = Brushes.White;
            btnSemesterInfo.Foreground = (Brush)bc.ConvertFrom("#FF1589FF");

            btnHome1.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnHome1.Foreground = Brushes.White;

            btnModules.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnModules.Foreground = Brushes.White;

            btnPersonalDetails.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnPersonalDetails.Foreground = Brushes.White;

            gridSemesterInfo.Visibility = Visibility.Visible;
            gridModuleDisplay.Visibility = Visibility.Hidden;
            gridModuleInfo.Visibility = Visibility.Hidden;
            gridWelcome.Visibility = Visibility.Hidden;
            gridPersonalDetails.Visibility = Visibility.Hidden;
            gridStudying.Visibility = Visibility.Hidden;
        }
        //Sorts grids and buttons to show the module grid
        private void moduleInfoClicked()
        {
            btnModules.Background = Brushes.White;
            btnModules.Foreground = (Brush)bc.ConvertFrom("#FF1589FF");

            btnHome1.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnHome1.Foreground = Brushes.White;

            btnSemesterInfo.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnSemesterInfo.Foreground = Brushes.White;

            btnPersonalDetails.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnPersonalDetails.Foreground = Brushes.White;

            btnStudyHistory.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnStudyHistory.Foreground = Brushes.White;

            gridSemesterInfo.Visibility = Visibility.Hidden;
            gridModuleDisplay.Visibility = Visibility.Hidden;
            gridModuleInfo.Visibility = Visibility.Visible;
            gridPersonalDetails.Visibility = Visibility.Hidden;
            gridStudying.Visibility = Visibility.Hidden;
        }
        //Sorts the grids and buttons to show the module display grid
        private void moduleDisplayClicked()
        {
            lblWeek.Content = "Week " + Convert.ToInt32(Math.Floor(Convert.ToDecimal(Math.Abs((Convert.ToDateTime(WorkerLists.semesterList.Start_Date) - DateTime.Now).Days) / 7)) + 1);
            btnHome1.Background = Brushes.White;
            btnHome1.Foreground = (Brush)bc.ConvertFrom("#FF1589FF");

            btnModules.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnModules.Foreground = Brushes.White;

            btnSemesterInfo.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnSemesterInfo.Foreground = Brushes.White;

            btnPersonalDetails.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnPersonalDetails.Foreground = Brushes.White;

            btnStudyHistory.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnStudyHistory.Foreground = Brushes.White;

            gridSemesterInfo.Visibility = Visibility.Hidden;
            gridModuleDisplay.Visibility = Visibility.Visible;
            gridModuleInfo.Visibility = Visibility.Hidden;
            gridWelcome.Visibility = Visibility.Collapsed;
            gridPersonalDetails.Visibility = Visibility.Hidden;
            gridStudying.Visibility = Visibility.Hidden;
            lblRemainingTotal.Visibility = Visibility.Hidden;
            gridStudySessionDisplay.Visibility = Visibility.Hidden;
            lblSelfStudyforCurrentweek.Visibility = Visibility.Hidden;
            lblremainingHoursFromPrevWeeks.Visibility = Visibility.Hidden;
            displayList();
        }

        //Validation method to alert the user if their input is incorrect, invalid or not allowed
        private void dateSemesterStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            string startDate;
            try
            {
                startDate = dateSemesterStart.SelectedDate.Value.ToShortDateString();
                lblStartDateError.Visibility = Visibility.Hidden;

            }
            catch (Exception)
            {
                lblStartDateError.Visibility = Visibility.Visible;
            }
        }
        //Validation method to alert the user if their input is incorrect, invalid or not allowed
        private void dateSemesterStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string startDate;
            try
            {
                startDate = dateSemesterStart.SelectedDate.Value.ToShortDateString();
                lblStartDateError.Visibility = Visibility.Hidden;
                if (dateSemesterStart.SelectedDate.HasValue == false)
                {


                    lblStartDateError.Visibility = Visibility.Visible;
                }

            }
            catch (Exception)
            {
                lblStartDateError.Visibility = Visibility.Visible;
            }
        }
        //Validation method to alert the user if their input is incorrect, invalid or not allowed
        private void btnToModuleDisplay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbModuleCode.Text.Length > 0 || txbModuleName.Text.Length > 0 || txbClassHours.Text.Length > 0 || txbNumCredits.Text.Length > 0)

                {
                    MessageBox.Show("You still have a Module to add.", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                //Checks if there is an object in the list
                if (Study_Library.WorkerLists.modulesList.Count == 0)
                {
                    MessageBox.Show("Please add your modules for this semester.", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    //If there is an object in te list then it will call this method and display the main menue
                    moduleDisplayClicked();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurred.\nPlease try again.", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnPersonalDetails_Click(object sender, RoutedEventArgs e)
        {
            personalDetailsClicked();

        }
        //Validation method to alert the user if their input is incorrect, invalid or not allowed
        private void txbFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string name = txbFirstName.Text;
                lblNameError.Visibility = Visibility.Hidden;
                if (newVal.checkStringInput(name) == false)
                {
                    lblNameError.Visibility = Visibility.Visible;
                }
                if (newVal.checkSpecial(name))
                {
                    lblNameError.Visibility = Visibility.Visible;
                }
                if (newVal.checkNumberOnly(name))
                {
                    lblNameError.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                lblNameError.Visibility = Visibility.Visible;
            }
        }
        private void txbSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string name = txbSurname.Text;
                lblSurnameError.Visibility = Visibility.Hidden;
                if (newVal.checkStringInput(name) == false)
                {
                    lblSurnameError.Visibility = Visibility.Visible;
                }
                if (newVal.checkSpecial(name))
                {
                    lblSurnameError.Visibility = Visibility.Visible;
                }
                if (newVal.checkNumberOnly(name))
                {
                    lblSurnameError.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                lblSurnameError.Visibility = Visibility.Visible;
            }
        }
        private void txbUniversityName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string name = txbUniversityName.Text;
                lblUniError.Visibility = Visibility.Hidden;
                if (newVal.checkStringInput(name) == false)
                {
                    lblUniError.Visibility = Visibility.Visible;
                }
                if (newVal.checkSpecial(name))
                {
                    lblUniError.Visibility = Visibility.Visible;
                }
                if (newVal.checkNumberOnly(name))
                {
                    lblUniError.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                lblUniError.Visibility = Visibility.Visible;
            }
        }
        private void txbModuleCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string Model = txbModuleCode.Text;
                lblModuleError.Visibility = Visibility.Hidden;
                if (newVal.checkStringInput(Model) == false)
                {
                    lblModuleError.Visibility = Visibility.Visible;
                }
                if (newVal.checkSpecial(Model) == true)
                {
                    lblModuleError.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                lblModuleError.Visibility = Visibility.Visible;
            }
        }
        private void txbModuleName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string Model = txbModuleName.Text;
                lblModuleNameError.Visibility = Visibility.Hidden;
                if (newVal.checkStringInput(Model) == false)
                {
                    lblModuleNameError.Visibility = Visibility.Visible;
                }
                if (newVal.checkSpecial(Model) == true)
                {
                    lblModuleNameError.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                lblModuleNameError.Visibility = Visibility.Visible;
            }
        }
        private void txbStudentNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string studentNumber = txbStudentNumber.Text;
                lblStudentNumberError.Visibility = Visibility.Hidden;

                if (newVal.isNumber(studentNumber) == false)
                {
                    lblStudentNumberError.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                lblStudentNumberError.Visibility = Visibility.Visible;
            }
        }
        private void txbStudyHours_TextChanged(object sender, TextChangedEventArgs e)
        {
            const int MAX_HOURS = 24;
            try
            {
                double sstudyHours = double.Parse(txbStudyHours.Text);
                lblstudyHourserror.Visibility = Visibility.Hidden;
                if (sstudyHours <= 0 || sstudyHours > MAX_HOURS)
                {
                    lblstudyHourserror.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                lblstudyHourserror.Visibility = Visibility.Visible;
            }
        }
        private void txbNumCredits_TextChanged(object sender, TextChangedEventArgs e)
        {
            const int MAX_CREDITS = 100;
            try
            {
                double Tax = double.Parse(txbNumCredits.Text);
                lblCreditsError.Visibility = Visibility.Hidden;
                if (Tax <= 0 || Tax > MAX_CREDITS)
                {
                    lblCreditsError.Visibility = Visibility.Visible;
                }

            }
            catch (Exception)
            {
                lblCreditsError.Visibility = Visibility.Visible;
            }
        }
        private void txbClassHours_TextChanged(object sender, TextChangedEventArgs e)
        {
            const int MAX_HOURS = 168;
            try
            {
                double classhours = double.Parse(txbClassHours.Text);
                lblClassErrors.Visibility = Visibility.Hidden;
                if (classhours <= 0 || classhours > MAX_HOURS)
                {
                    lblClassErrors.Visibility = Visibility.Visible;
                }

            }
            catch (Exception)
            {
                lblClassErrors.Visibility = Visibility.Visible;
            }
        }
        private void txbNumWeeks_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            const int MAX_WEEKS = 52;
            try
            {
                double numWeeks = double.Parse(txbNumWeeks.Text);
                lblNumWeeksError.Visibility = Visibility.Hidden;
                if (numWeeks <= 0 || numWeeks > MAX_WEEKS)
                {
                    lblNumWeeksError.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                lblNumWeeksError.Visibility = Visibility.Visible;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////

        private void btnGetStarted_Click(object sender, RoutedEventArgs e)
        {
            getStartedClick();
        }
        //Method to adjust the colours of buttons and fonts to indicate which form is in use
        private void getStartedClick()
        {
            btnPersonalDetails.Background = Brushes.White;
            btnPersonalDetails.Foreground = (Brush)bc.ConvertFrom("#FF1589FF");

            btnModules.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnModules.Foreground = Brushes.White;

            btnSemesterInfo.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnSemesterInfo.Foreground = Brushes.White;

            btnHome1.Background = (Brush)bc.ConvertFrom("#FF1589FF");
            btnHome1.Foreground = Brushes.White;

            gridSemesterInfo.Visibility = Visibility.Hidden;
            gridModuleDisplay.Visibility = Visibility.Hidden;
            gridModuleInfo.Visibility = Visibility.Hidden;
            gridPersonalDetails.Visibility = Visibility.Visible;
            gridStudying.Visibility = Visibility.Hidden;
            gridWelcome.Visibility = Visibility.Hidden;
        }
        private void selectedModulelist()
        {
            try
            {
                //Checks if item is selected
                if (lstModuleCode.SelectedIndex != -1)
                {
                    if (WorkerLists.studyingList != null)
                    {
                        //Creates tuple object
                        Tuple<string, string, string, string, string, string> newTuple = UtilClass.selectedModuleDisplay(selectedModuleCOde);
                        //set GUI components to items of the tuple
                        lblModuleCodeD.Content = newTuple.Item1;
                        lblModuleNameD.Content = newTuple.Item2;
                        lblSelfStudyD.Content = newTuple.Item3;
                        lblHoursThisweek.Content = newTuple.Item4;
                        lblHoursFromPrev.Content = newTuple.Item5;
                        lblSelfStudyforCurrentweek_2.Content = newTuple.Item6;
                        //Reset forms
                        gridStudying.Visibility = Visibility.Visible;
                        gridModuleDisplay.Visibility = Visibility.Hidden;
                        gridWelcome.Visibility = Visibility.Hidden;
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

        private void btnGetStarted_Click_1(object sender, RoutedEventArgs e)
        {
            //Resets components to show the correct forms etc
            gridWelcome.Visibility = Visibility.Hidden;
            personalDetailsClicked();
        }

        private void btnUpdateHours_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                const int MAX_HOURS = 24;
                //Get data from gui components
                int studyHours = Convert.ToInt32(txbStudyHours.Text);
                DateTime? selectedDate = dateStudyDate.SelectedDate;
                //getting date 
                string studyDate = selectedDate.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //Valdiates that fields are correct
                if (studyHours > 0 && studyHours <= MAX_HOURS && selectedDate.HasValue)
                {
                    //Creates instance of study and adds it to the list
                    Study_Library.Studying newStudy = new Studying(studyHours, UtilClass.findSelectedModule(selectedModuleCOde).Item1, studyDate);
                    WorkerLists.studyingList.Add(newStudy);

                    selectedModulelist();
                    //Confirmation, reset form and display the added study session to listbox
                    MessageBox.Show("Study session recorded.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);

                    dateStudyDate.SelectedDate = null;
                    txbStudyHours.Clear();
                    dispalyStudysession();
                    lblstudyHourserror.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show("Please enter the correct values for this study session.", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurred. Please try again.", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void lstModuleCode_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            addStudySession();
        }
        //Method to check if an item in the list is selected and add a study session to that module
        private void addStudySession()
        {
            //Checks if item is selected
            if (lstModuleCode.SelectedIndex != -1)
            {
                //Gets selected item
                string selectedModuleName = lstModuleCode.SelectedItem.ToString();

                selectedModuleCOde = selectedModuleName;

                selectedModulelist();
            }
            else
            {
                MessageBox.Show("Please select a module.", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        //Methods which checks if "Enter" is clicked to focus to the next component
        private void txbModuleCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                txbModuleName.Focus();
            }
        }
        private void txbModuleName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                txbNumCredits.Focus();
            }
        }
        private void txbNumCredits_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                txbClassHours.Focus();
            }
        }
        private void txbClassHours_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                btnAddModule.Focus();
            }
        }
        private void txbFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                txbSurname.Focus();
            }
        }
        private void txbSurname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                txbUniversityName.Focus();
            }
        }
        private void txbUniversityName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                btnToSemesterGrid.Focus();
            }
        }
        ///////////////////////////////////////////////////////////////////////////
        private void btnAddModule1_Click(object sender, RoutedEventArgs e)
        {
            moduleInfoClicked();
        }
        private void btnDeleteModule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstModuleCode.SelectedIndex != -1)
                {
                    //creates instance of utilclass
                    UtilClass newUtil = new UtilClass();
                    //calls method to delete module
                    newUtil.deleteModule(lstModuleCode.SelectedIndex, lstModuleCode.SelectedItem.ToString());
                    //Updates list
                    displayList();
                }
                else
                {
                    MessageBox.Show("Please select a module.", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception)
            { MessageBox.Show("An unexpected error occurred", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning); }
        }
        private void btnStudyHistory_Click(object sender, RoutedEventArgs e)
        {
            //Validates that there is an object in the list
            if (WorkerLists.studyingList == null || WorkerLists.studyingList.Count == 0)
            {
                MessageBox.Show("You have no study sessions to view. Please record a study a session.", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                //Sets forms and buttons to alert the user they are in the correct form
                btnStudyHistory.Background = Brushes.White;
                btnStudyHistory.Foreground = (Brush)bc.ConvertFrom("#FF1589FF");

                btnModules.Background = (Brush)bc.ConvertFrom("#FF1589FF");
                btnModules.Foreground = Brushes.White;

                btnSemesterInfo.Background = (Brush)bc.ConvertFrom("#FF1589FF");
                btnSemesterInfo.Foreground = Brushes.White;

                btnPersonalDetails.Background = (Brush)bc.ConvertFrom("#FF1589FF");
                btnPersonalDetails.Foreground = Brushes.White;

                btnHome1.Background = (Brush)bc.ConvertFrom("#FF1589FF");
                btnHome1.Foreground = Brushes.White;
                //Sets forms
                gridModuleDisplay.Visibility = Visibility.Hidden;
                gridModuleInfo.Visibility = Visibility.Hidden;
                gridPersonalDetails.Visibility = Visibility.Hidden;
                gridSemesterInfo.Visibility = Visibility.Hidden;
                gridStudying.Visibility = Visibility.Hidden;
                gridStudySessionDisplay.Visibility = Visibility.Visible;
            }
        }
        private void btnAddStudySession_Click_1(object sender, RoutedEventArgs e)
        {
            addStudySession();
        }

        private void btnViewStudyDetails_Click(object sender, RoutedEventArgs e)
        {
            viewHourBreakDown();
        }
        private void viewHourBreakDown()
        {
            try
            {
                //Checks if item is selected
                if (lstModuleHours.SelectedIndex != -1)
                {
                    //Finds the related module with its study hours
                    lstModuleCode.SelectedIndex = lstModuleHours.SelectedIndex;
                    //checks if there is a studying object in the list
                    if (WorkerLists.studyingList != null)
                    {
                        //Creates tuple object
                        Tuple<string, string, string, string, string, string> newTuple = UtilClass.selectedModuleDisplay(lstModuleCode.SelectedItem.ToString());
                        //set GUI components to items of the tuple
                        lblSelfStudyforCurrentweek.Content = newTuple.Item4;
                        lblremainingHoursFromPrevWeeks.Content = newTuple.Item5;
                        lblRemainingTotal.Content = newTuple.Item6;

                        //shows the labels
                        lblSelfStudyforCurrentweek.Visibility = Visibility.Visible;
                        lblremainingHoursFromPrevWeeks.Visibility = Visibility.Visible;
                        lblRemainingTotal.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    MessageBox.Show("Please select a remaining study hours item.", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurred. Please try again.", "ALERT", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void lstModuleHours_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            viewHourBreakDown();
        }

        private void rectPrint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //Constructor and print method
                UtilClass newUtil = new UtilClass();
                newUtil.PrintStudySessions();
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurred", "Smart Study Planner", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void rectSave_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (WorkerLists.studyingList.Count > 0)
            {
                UtilClass newUtil = new UtilClass();
                newUtil.writeToFile();
            }
            else
            {
                MessageBox.Show("You have no Study sessions to save... :(", "Smart Study Planner", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txbStudentNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                txbFirstName.Focus();
            }
        }

        private void txbNumWeeks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                dateSemesterStart.Focus();
            }
        }

        private void dateSemesterStart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                btnToModuleInfo.Focus();
            }
        }
    }
}
