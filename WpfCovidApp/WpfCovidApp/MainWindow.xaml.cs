using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfCovidApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /****************************
         ****************************
         ******** PROPERTIES ********
         ****************************
         ****************************/
        // Variabelen initialiseren
        int aantalFouten = 0;
        // Filter sleutelwoorden        
        enum FilterType
        {
            all,
            id,
            fn
        }
        // Bewaar de personen in de lijst
        List<Person> contacten = new List<Person>();
        // Onthoud de geselecteerde persoon
        Person geselecteerdePersoon;
        public MainWindow()
        {
            InitializeComponent();
           // aantal buttons disablen aan het begin van het programma
            btnChange.IsEnabled = false;
            btnSave.IsEnabled = false;
            btnSaveCancel.IsEnabled = false;
            btnRemove.IsEnabled = false;
        }
        /****************************
         ****************************
         ****** COMMON FUNCTIONS ****
         ****************************
         ****************************/
        /*
         * Refresh de contactenlijst gebaseerd op de filter
         */
        private void RefreshContactList()
        {
            // Listbox volledig leeg maken
            lbxOutput.Items.Clear();
            // Check of er een filter is
            FilterType fltr;
            if (txtZoek.Text != "")
                fltr = Int64.TryParse(txtZoek.Text, out long num) ? FilterType.id : FilterType.fn;
            else
                fltr = FilterType.all;
            // Geef de gevraagde personen weer in de listbox
            foreach (Person p in contacten)
                if (FilteredPerson(fltr, p))
                {
                    // lbxOutput.Items.Add(p.DisplayPerson());
                    ListBoxItem li = new ListBoxItem();
                    li.Content = p.DisplayPerson();
                    li.Background = (p.AlreadyContacted) ? Brushes.LightGreen : Brushes.Red; // Colors.Red;
                    lbxOutput.Items.Add(li);
                }
        }
        /*
         * Helper method: Checkt of de persoon gevraagd is
         */
        private bool FilteredPerson(FilterType fltr, Person p)
        {
            string strZoek = txtZoek.Text;
            // Als het id of fn is, dan mag strZoek niet langer zijn dan de corresponderende waarde
            if ((fltr.Equals(FilterType.id) && p.IdNo.ToString().Length >= strZoek.Length
                && strZoek.Equals(p.IdNo.ToString().Substring(0, strZoek.Length)))
                || (fltr.Equals(FilterType.fn) && p.FirstName.Length >= strZoek.Length
                && strZoek.ToLower().Equals(p.FirstName.Substring(0, strZoek.Length).ToLower()))
                || fltr.Equals(FilterType.all))
                return true;
            else
                return false;
        }
        /*
         * Maak het formulier klaar om de volgende mogelijke persoon toe te voegen
         */
        private void EmptyForm()
        {
            //invulformulier velden leeg maken
            txtRRN.Text = "";
            txtVoornaam.Text = "";
            txtFamilienaam.Text = "";
            dpiDatum.Text = "";
            rdnVrouw.IsChecked = false;
            rdnMan.IsChecked = false;
            txtStraat.Text = "";
            txtHuisnummer.Text = "";
            txtPostcode.Text = "";
            txtGemeente.Text = "";
            txtTelefoonnummer.Text = "";
            txtEmail.Text = "";
            txtAantal.Text = "";
            cbxType.Text = "";
            rdnGecontacteerdJa.IsChecked = false;
            rdnGecontacteerdNee.IsChecked = false;
            // Maak velden terug default indien nodig
            txtRRN.Background = Brushes.White;
            txtVoornaam.Background = Brushes.White;
            txtFamilienaam.Background = Brushes.White;
            dpiDatum.Background = Brushes.White;
            rdnVrouw.Background = Brushes.White;
            rdnMan.Background = Brushes.White;
            txtStraat.Background = Brushes.White;
            txtHuisnummer.Background = Brushes.White;
            txtPostcode.Background = Brushes.White;
            txtGemeente.Background = Brushes.White;
            txtTelefoonnummer.Background = Brushes.White;
            txtEmail.Background = Brushes.White;
            txtAantal.Background = Brushes.White;
            cbxType.Background = Brushes.White;
            rdnGecontacteerdJa.Background = Brushes.White;
            rdnGecontacteerdNee.Background = Brushes.White;
            // Wis meldingen indien nodig
            lblHMI.Content = "";
            lblHMI.Foreground = Brushes.Black;
            lblHMI2.Content = "";
            lblHMI2.Foreground = Brushes.Black;
        }
        /*
         * Controleer of het formulier volledig ingevuld is
         */
        private void ValidateInputForm()
        {
            // wis HMI interfaces
            lblHMI.Content = "";
            lblHMI2.Content = "";
            // zet aantal fouten terug op 0 alvorens te valideren
            aantalFouten = 0;
            // Check invulformulier en alle inputveldenn 
            // rijksregisternummer valideren
            // https://docs.microsoft.com/en-us/dotnet/api/system.char.isnumber?redirectedfrom=MSDN&view=net-5.0#System_Char_IsNumber_System_Char
            int aantalKarakters2 = txtRRN.Text.Length;
            bool allemaalNummers = false;   // alternatief: long.TryParse(txtRRN.Text, out long num)
            int aantalFoutenRRN = 0;
            for (int i = 0; i < aantalKarakters2; i++)
            {
                if (Char.IsNumber(txtRRN.Text, i) == true)
                {
                    allemaalNummers = true;
                }
                else
                {
                    allemaalNummers = false;
                    aantalFoutenRRN++;
                }
            }
            if (aantalKarakters2 == 11 && allemaalNummers == true && aantalFoutenRRN == 0)
            {
                txtRRN.Background = Brushes.White;
            }
            else
            {
                txtRRN.Background = Brushes.Red;
                lblHMI2.Content = "Rijksregisternummer moet 11 cijfers bevatten";
                aantalFouten++;
            }
            // voornaam valideren
            if (txtVoornaam.Text == "")
            {
                txtVoornaam.Background = Brushes.Red;
                lblHMI2.Content = "Voornaam moet ingevuld zijn";
                aantalFouten++;
            }
            else
            {
                txtVoornaam.Background = Brushes.White;
            }
            if (txtFamilienaam.Text == "")
            {
                txtFamilienaam.Background = Brushes.Red;
                lblHMI2.Content = "Familienaam moet ingevuld zijn";
                aantalFouten++;
            }
            else
            {
                txtFamilienaam.Background = Brushes.White;
            }
            if (dpiDatum.Text == "")
            {
                dpiDatum.Background = Brushes.Red;
                lblHMI2.Content = "Geboortedatum moet ingevuld zijn";
                aantalFouten++;
            }
            else
            {
                dpiDatum.Background = Brushes.White;
            }
            if (rdnVrouw.IsChecked == false && rdnMan.IsChecked == false)
            {
                rdnVrouw.Background = Brushes.Red;
                rdnMan.Background = Brushes.Red;
                lblHMI2.Content = "Kies een geslacht";
                aantalFouten++;
            }
            else
            {
                rdnVrouw.Background = Brushes.White;
                rdnMan.Background = Brushes.White;
            }
            if (txtStraat.Text == "")
            {
                txtStraat.Background = Brushes.Red;
                lblHMI2.Content = "Straatnaam moet ingevuld zijn";
                aantalFouten++;
            }
            else
            {
                txtStraat.Background = Brushes.White;
            }
            if (txtHuisnummer.Text == "")
            {
                txtHuisnummer.Background = Brushes.Red;
                lblHMI2.Content = "Huisnummer moet ingevuld zijn";
                aantalFouten++;
            }
            else
            {
                txtHuisnummer.Background = Brushes.White;
            }
            // Postcode checken
            int aantalKarakters = txtPostcode.Text.Length;
            if (aantalKarakters == 4 && (Char.IsNumber(txtPostcode.Text, 0) == true) && (Char.IsNumber(txtPostcode.Text, 1) == true)
                && (Char.IsNumber(txtPostcode.Text, 2) == true) && (Char.IsNumber(txtPostcode.Text, 3) == true))
            {
                txtPostcode.Background = Brushes.White;
            }
            else
            {
                txtPostcode.Background = Brushes.Red;
                lblHMI2.Content = "Postcode moet uit 4 cijfers bestaan";
                aantalFouten++;
            }
            if (txtGemeente.Text == "")
            {
                txtGemeente.Background = Brushes.Red;
                lblHMI2.Content = "Gemeente moet ingevuld zijn";
                aantalFouten++;
            }
            else
            {
                txtGemeente.Background = Brushes.White;
            }
            if (txtTelefoonnummer.Text == "")
            {
                txtTelefoonnummer.Background = Brushes.Red;
                lblHMI2.Content = "Telefoonnummer moet ingevuld zijn";
                aantalFouten++;
            }
            else
            {
                txtTelefoonnummer.Background = Brushes.White;
            }
            if (txtEmail.Text == "")
            {
                txtEmail.Background = Brushes.Red;
                lblHMI2.Content = "Email moet ingevuld zijn";
                aantalFouten++;
            }
            else
            {
                txtEmail.Background = Brushes.White;
            }
            if (txtAantal.Text == "")
            {
                txtAantal.Background = Brushes.Red;
                lblHMI2.Content = "Aantal Gezinsleden moet ingevuld zijn";
                aantalFouten++;
            }
            else
            {
                txtAantal.Background = Brushes.White;
            }
            if (cbxType.Text == "")
            {
                cbxType.Background = Brushes.Red;
                lblHMI2.Content = "Maak een keuze bij Type Contact";
                aantalFouten++;
            }
            else
            {
                cbxType.Background = Brushes.White;
            }
            if (rdnGecontacteerdJa.IsChecked == false && rdnGecontacteerdNee.IsChecked == false)
            {
                rdnGecontacteerdJa.Background = Brushes.Red;
                rdnGecontacteerdNee.Background = Brushes.Red;
                lblHMI2.Content = "Duid aan of de persoon reeds gecontacteerd is";
                aantalFouten++;
            }
            else
            {
                rdnGecontacteerdJa.Background = Brushes.White;
                rdnGecontacteerdNee.Background = Brushes.White;
            }
        }
        /****************************
         ****************************
         ****** CLICK FUNCTIONS *****
         ****************************
         ****************************/
        /*
         * Voeg een persoon toe aan de contactenlijst 
         */
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Check of alle inputs correct ingegeven zijn
            ValidateInputForm();
            //output naar klant toe. Als alles goed is bedanken, anders het aantal fouten vermelden
            if (aantalFouten == 0 && !PersonExists(txtRRN.Text))
            {                                                   // geselecteerdePersoon.Birthday = dpiDatum.SelectedDate.Value;
                // Create a person
                Person person = new Person(long.Parse(txtRRN.Text), txtVoornaam.Text, txtFamilienaam.Text, dpiDatum.SelectedDate.Value,
                    rdnVrouw.IsChecked == true ? "vrouw" : "man", new string[] { txtStraat.Text, txtHuisnummer.Text, txtPostcode.Text, txtGemeente.Text },
                    long.Parse(txtTelefoonnummer.Text), txtEmail.Text, Int16.Parse(txtAantal.Text), cbxType.Text, rdnGecontacteerdJa.IsChecked == true ? true : false);
                contacten.Add(person);
                RefreshContactList();
                // Invulformulier velden leeg maken
                EmptyForm();
                lblHMI.Content = "OK Contact is toegevoegd";
            }
            else
            {
                lblHMI.Foreground = Brushes.Red;
                if (!long.TryParse(txtRRN.Text, out long num))
                    lblHMI.Content = ($"Ongeldig rijksregisternummer");
                else if (aantalFouten == 0 && PersonExists(txtRRN.Text))
                    lblHMI.Content = ($"Deze persoon zit al in de lijst");
                else
                    lblHMI.Content = ($"Dit  formulier bevat {aantalFouten} fout(en)");
            }
        }
        /*
         * Formulier van een contact wijzigen
         */
        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (lbxOutput.SelectedIndex >= 0)
            {
                // Activeer de knop Opslaan
                btnSave.IsEnabled = true;
                btnSaveCancel.IsEnabled = true;
                // Andere knoppen uitschakelen
                btnAdd.IsEnabled = false;
                btnChange.IsEnabled = false;
                btnRemove.IsEnabled = false;
                btnShowAll.IsEnabled = false;
                btnClearAll.IsEnabled = false;
                btnImport.IsEnabled = false;
                btnExport.IsEnabled = false;
                // Invulformulier velden leeg maken
                EmptyForm();
                // Upload de persoonsgegevens in het formulier
                txtRRN.Text = geselecteerdePersoon.IdNo.ToString();
                txtVoornaam.Text = geselecteerdePersoon.FirstName;
                txtFamilienaam.Text = geselecteerdePersoon.LastName;
                dpiDatum.SelectedDate = geselecteerdePersoon.Birthday;
                if (geselecteerdePersoon.Gender.Equals("vrouw"))
                {
                    rdnVrouw.IsChecked = true;
                    rdnMan.IsChecked = false;
                }
                else
                {
                    rdnVrouw.IsChecked = false;
                    rdnMan.IsChecked = true;
                }
                txtStraat.Text = geselecteerdePersoon.Address[0];
                txtHuisnummer.Text = geselecteerdePersoon.Address[1];
                txtPostcode.Text = geselecteerdePersoon.Address[2];
                txtGemeente.Text = geselecteerdePersoon.Address[3];
                txtTelefoonnummer.Text = geselecteerdePersoon.PhoneNumber.ToString();
                txtEmail.Text = geselecteerdePersoon.Email;
                txtAantal.Text = geselecteerdePersoon.NumberFamilyMembers.ToString();
                cbxType.Text = geselecteerdePersoon.TypeContact;
                if (geselecteerdePersoon.AlreadyContacted)
                {
                    rdnGecontacteerdJa.IsChecked = true;
                    rdnGecontacteerdNee.IsChecked = false;
                }
                else
                {
                    rdnGecontacteerdJa.IsChecked = false;
                    rdnGecontacteerdNee.IsChecked = true;
                }
                //aanpassen, dit kan manueel in het invulformulier
                lblHMI.Content = ($"Wijzig de gegevens.");
            }
        }
        /*
         * Schrijf de nieuwe inputs van de persoon weg
         */
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Andere knoppen terug aanschakelen
            btnAdd.IsEnabled = true;
            btnShowAll.IsEnabled = true;
            btnClearAll.IsEnabled = true;
            btnImport.IsEnabled = true;
            btnExport.IsEnabled = true;
            // Check of alle inputs correct ingegeven zijn
            ValidateInputForm();
            if (aantalFouten == 0 && (geselecteerdePersoon.IdNo == long.Parse(txtRRN.Text) || !PersonExists(txtRRN.Text)))
            {
                // Schrijf de nieuwe inputs weg
                geselecteerdePersoon.IdNo = long.Parse(txtRRN.Text);
                geselecteerdePersoon.FirstName = txtVoornaam.Text;
                geselecteerdePersoon.LastName = txtFamilienaam.Text;
                geselecteerdePersoon.Birthday = dpiDatum.SelectedDate.Value;
                geselecteerdePersoon.Gender = rdnVrouw.IsChecked == true ? "vrouw" : "man";
                geselecteerdePersoon.Address = new string[] { txtStraat.Text, txtHuisnummer.Text, txtPostcode.Text, txtGemeente.Text };
                geselecteerdePersoon.PhoneNumber = long.Parse(txtTelefoonnummer.Text);
                geselecteerdePersoon.Email = txtEmail.Text;
                geselecteerdePersoon.NumberFamilyMembers = Int16.Parse(txtAantal.Text);
                geselecteerdePersoon.TypeContact = cbxType.Text;
                geselecteerdePersoon.AlreadyContacted = rdnGecontacteerdJa.IsChecked == true;
                // Contactlijst refreshen
                RefreshContactList();
                // Invulformulier velden leeg maken
                EmptyForm();
                lblHMI.Content = ($"Wijzigingen zijn opgeslagen.");
            }
            else
            {
                lblHMI.Foreground = Brushes.Red;
                if (!long.TryParse(txtRRN.Text, out long num))
                    lblHMI.Content = ($"Ongeldig rijksregisternummer");
                else if (aantalFouten == 0 && PersonExists(txtRRN.Text))
                    lblHMI.Content = ($"Deze persoon zit al in de lijst");
                else
                    lblHMI.Content = ($"Dit  formulier bevat {aantalFouten} fout(en)");
            }
        }
        /*
         * Annuleer de wijzigingen
         */
        private void btnSaveCancel_Click(object sender, RoutedEventArgs e)
        {
            // Andere knoppen terug aanschakelen
            btnAdd.IsEnabled = true;
            btnShowAll.IsEnabled = true;
            btnClearAll.IsEnabled = true;
            btnImport.IsEnabled = true;
            btnExport.IsEnabled = true;
            // Contactlijst refreshen
            RefreshContactList();
            // Invulformulier velden leeg maken
            EmptyForm();
            lblHMI.Content = ($"Wijzigingen zijn geannuleerd.");
        }
        /*
         * Verwijder de geselecteerde persoon van de contactenlijst 
         */
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lbxOutput.SelectedIndex >= 0)
            {
                contacten.Remove(geselecteerdePersoon);
                lbxOutput.Items.Remove(lbxOutput.SelectedItem);
                // Listbox refreshen
                RefreshContactList();
                lblHMI.Content = ($"De geselecteerde persoon is gewist.");
            }
        }
        /*
         * Geef iedereen weer en kuis txtZoek op
         */
        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            txtZoek.Text = "";
            lblHMI.Content = ($"Alle personen worden nu weergegeven.");
        }
        /*
         * Wis idereen en kuis txtZoek op
         */
        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            contacten.Clear();
            RefreshContactList();
            lblHMI.Content = ($"Alle personen zijn gewist.");
        }
        /*
         * Importeer het csv bestand
         * Geef de gegevens in de listbox weer
         */
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = @"C:\";
            dialog.Filter = "Csv Files|*.csv;*.CSV";
            if (dialog.ShowDialog() == true)
            {
                Stream fileStream = dialog.OpenFile();
                try
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string s = "";
                        while ((s = reader.ReadLine()) != null)
                        {
                            string[] values = s.Split(',');
                            // Check of de lijn alle data van persoon bevat
                            bool completePersonData = true;
                            foreach (string value in values)
                                if (value == "")
                                {
                                    completePersonData = false;
                                    break;
                                }
                            // Check of het rijksregisternummer geldig is
                            if (values[0].Length != 11 && long.TryParse(values[0], out long num))
                                completePersonData = false;
                            // Check of deze persoon al in de lijst zit
                            if (completePersonData && !PersonExists(values[0]))
                            {
                                Person person = new Person(long.Parse(values[0]), values[1], values[2], DateTime.Parse(values[3]), values[4],
                                    new string[] { values[5], values[6], values[7], values[8] }, long.Parse(values[9]), values[10],
                                    Int16.Parse(values[11]), values[12], values[13].ToLower().Equals("ja") ? true : false);
                                contacten.Add(person);
                            }
                        }
                    }
                    RefreshContactList();
                    lblHMI.Content = ($"De lijst is geimporteerd.");
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }
        /*
         * Helper method: Controleert of een persoon al in de lijst zit
         */
        private bool PersonExists(string id)
        {
            foreach (Person p in contacten)
                if (p.IdNo == long.Parse(id))
                    return true;
            return false;
        }
        /*
         * Exporteren van de bestandsnamen naar csv file
         */
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                string allText = "";
                foreach (ListBoxItem line in lbxOutput.Items)
                    allText += line.Content.ToString() + "\n";
                File.WriteAllText(dialog.FileName, allText.Replace(" ", ","));
                lblHMI.Content = ($"De lijst is geëxporteerd.");
            }
        }
        /****************************
         ****************************
         **** CHANGED FUNCTIONS *****
         ****************************
         ****************************/
        /*
         * Schakel de knoppen in/uit 
         */
        private void lbxOutput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EmptyForm();
            if (lbxOutput.SelectedIndex >= 0)
            {
                CacheSelectedPerson();
                btnChange.IsEnabled = true;
                btnRemove.IsEnabled = true;
            }
            else
            {
                geselecteerdePersoon = null;
                btnChange.IsEnabled = false;
                btnSave.IsEnabled = false;
                btnSaveCancel.IsEnabled = false;
                btnRemove.IsEnabled = false;
            }
        }
        /*
         * Helper method: Vindt de geselecteerde persoon en kent hem toe aan geselecteerdePersoon
         */
        private void CacheSelectedPerson()
        {
            // Grijp de persoon via rijksregisternummer; uniek 
            ListBoxItem selectedItem = (ListBoxItem)lbxOutput.SelectedItem;
            long selectedId = long.Parse(selectedItem.Content.ToString().Substring(0, 11));
            foreach (Person p in contacten)
                if (p.IdNo == selectedId)
                    geselecteerdePersoon = p;
        }
        /*
         * Filtering gebeurt automatisch in RefreshContactList()
         */
        private void txtZoek_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshContactList();
            // Wis iedereen enkel iedereen weergegeven wordt; conflict vermijden
            btnClearAll.IsEnabled = !(txtZoek.Text != "");
        }
        /****************************
         ****************************
         ***** FOCUS FUNCTIONS ******
         ****************************
         ****************************/
        /*
         * Focus changed; voer de nodige taken uit
         */
        private void EmptyForm(object sender, RoutedEventArgs e)
        {
            EmptyForm();
        }
    }
}