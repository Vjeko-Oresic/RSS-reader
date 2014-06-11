using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ProjektII
{
    public partial class Form1 : Form
    {
        XmlTextReader rssReader;
        XmlDocument rssDoc;
        XmlNode nodeRss;
        XmlNode nodeChannel;
        XmlNode nodeItem;
        ListViewItem rowList;

        public Form1()
        {
            InitializeComponent();
        }

        private void b_Ucitaj_Click(object sender, EventArgs e)
        {
            //Čišćenje liste ako u njoj postoje već neki Items
            listVijest.Items.Clear();
            this.Cursor = Cursors.WaitCursor;
            //Kreiranje XmlTextReader za uneseni link
            rssReader = new XmlTextReader(txtLink.Text);
            rssDoc = new XmlDocument();
            //Load sadržaja XML-a u XmlDocument
            rssDoc.Load(rssReader);
            //Petlja za <rss>
            for (int i = 0; i < rssDoc.ChildNodes.Count; i++)
            {
                if (rssDoc.ChildNodes[i].Name == "rss")
                {
                    nodeRss = rssDoc.ChildNodes[i];
                }
            }
            //Petlja za <channel>
            for (int i = 0; i < nodeRss.ChildNodes.Count; i++)
            {
                if (nodeRss.ChildNodes[i].Name == "channel")
                {
                    nodeChannel = nodeRss.ChildNodes[i];
                }
            }
            //Labele s informacijama
            lblNaziv.Text = "Naziv: " + nodeChannel["title"].InnerText;
            lblOpis.Text = "Opis: " + nodeChannel["description"].InnerText;
            //Petlja za <item>
            for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
            {
                if (nodeChannel.ChildNodes[i].Name == "item")
                {
                    nodeItem = nodeChannel.ChildNodes[i];
                    //Kreira nove redove u ListView s informacijama iz nodeItem
                    rowList = new ListViewItem();
                    rowList.Text = nodeItem["title"].InnerText;
                    rowList.SubItems.Add(nodeItem["link"].InnerText);
                    listVijest.Items.Add(rowList);
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void listVijest_DoubleClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(listVijest.SelectedItems[0].SubItems[1].Text);
        }

        private void listVijest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listVijest.SelectedItems.Count == 1)
            {
                //Petlja kroz <channel> dok ne naiđe na <item>
                for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
                {
                    if (nodeChannel.ChildNodes[i].Name == "item")
                    {
                        nodeItem = nodeChannel.ChildNodes[i];
                        //Ako je <title> jednak selektiranom u ListView prikazat će description u txtOpis
                        if (nodeItem["title"].InnerText == listVijest.SelectedItems[0].Text)
                        {
                            txtOpis.Text = nodeItem["description"].InnerText;
                            break;
                        }
                    }
                }
            }
        }

    }
}
