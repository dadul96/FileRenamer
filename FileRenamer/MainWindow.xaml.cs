/*
Title: File Renamer
Description: This program automatically renames files in the selected directory and the subdirectories, if selected so. It can replace spaces or hyphens with underscores.
Author: Daniel Duller
Version: 1.0.0
Creation date: 31.07.2019
Last change: 01.08.2019
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;

namespace FileRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static string[] files;

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();    //open the FolderBrowserDialog

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        pathText.Text = fbd.SelectedPath;   //set the TextBox-text to the directory path

                        //count the files in the selected path and the subdirectories, if selected so:
                        string[] tempFiles;

                        if (subdirectoriesCheckbox.IsChecked == true)
                        {
                            tempFiles = Directory.GetFiles(fbd.SelectedPath, "*", SearchOption.AllDirectories);
                        }
                        else
                        {
                            tempFiles = Directory.GetFiles(fbd.SelectedPath);
                        }

                        System.Windows.MessageBox.Show(("Files found: " + tempFiles.Length.ToString()), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void pathText_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //enables the replace-buttons, if the TextBox contains a string:
                if (pathText.Text != "")
                {
                    renameButton1.IsEnabled = true;
                    renameButton2.IsEnabled = true;
                }
                else
                {
                    renameButton1.IsEnabled = false;
                    renameButton2.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void renameButton1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //get the files from the path, which is in the TextBox:
                if (subdirectoriesCheckbox.IsChecked == true)
                {
                    files = Directory.GetFiles(pathText.Text, "*", SearchOption.AllDirectories);
                }
                else
                {
                    files = Directory.GetFiles(pathText.Text);
                }

                renameOperation(" ");    //run the renaming function and replace the space characters (" ")
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void renameButton2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //get the files from the path, which is in the TextBox:
                if (subdirectoriesCheckbox.IsChecked == true)
                {
                    files = Directory.GetFiles(pathText.Text, "*", SearchOption.AllDirectories);
                }
                else
                {
                    files = Directory.GetFiles(pathText.Text);
                }

                renameOperation("-");    //run the renaming function and replace the hyphen characters ("-")
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void renameOperation(string elementToReplace)    //renaming function
        {
            try
            {
                MessageBoxResult renameQuestionResult = System.Windows.MessageBox.Show("Do you really want to rename the files in the selected folder?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (renameQuestionResult == MessageBoxResult.Yes)
                {
                    int renamedFilesCount = 0;

                    foreach (string oldFilePath in files)   //replacing algorithm
                    {
                        string newFilePath = oldFilePath;
                        int namePos = 0;

                        namePos = newFilePath.LastIndexOf("\\");

                        if (namePos > 0)
                        {
                            string tempString = "";
                            string tempStringOld = "";

                            tempString = newFilePath.Remove(0, namePos + 1);
                            tempStringOld = tempString;
                            tempString = tempString.Replace(elementToReplace, "_");

                            if (tempStringOld != tempString)
                            {
                                renamedFilesCount++;
                            }

                            newFilePath = newFilePath.Remove(namePos + 1);
                            newFilePath = newFilePath + tempString;

                            File.Move(oldFilePath, newFilePath);
                        }
                    }
                    System.Windows.MessageBox.Show(("Files renamed: " + renamedFilesCount.ToString()), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
