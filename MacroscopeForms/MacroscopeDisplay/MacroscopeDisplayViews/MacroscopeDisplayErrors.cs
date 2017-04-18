﻿/*

	This file is part of SEOMacroscope.

	Copyright 2017 Jason Holland.

	The GitHub repository may be found at:

		https://github.com/nazuke/SEOMacroscope

	Foobar is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	Foobar is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Foobar.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;

namespace SEOMacroscope
{

  /// <summary>
  /// Description of MacroscopeDisplayErrors.
  /// </summary>

  public sealed class MacroscopeDisplayErrors : MacroscopeDisplayListView
  {

    /**************************************************************************/

    public MacroscopeDisplayErrors ( MacroscopeMainForm MainForm, ListView lvListView )
      : base( MainForm, lvListView )
    {

      this.MainForm = MainForm;
      this.lvListView = lvListView;

      if( this.MainForm.InvokeRequired )
      {
        this.MainForm.Invoke(
          new MethodInvoker (
            delegate
            {
              this.ConfigureListView();
            }
          )
        );
      }
      else
      {
        this.ConfigureListView();
      }

    }

    /**************************************************************************/

    protected override void ConfigureListView ()
    {
      if( !this.ListViewConfigured )
      {
        this.ListViewConfigured = true;
      }
    }

    /**************************************************************************/

    public override void RenderListView ( MacroscopeDocumentCollection DocCollection )
    {

      if( DocCollection.CountDocuments() == 0 )
      {
        return;
      }
            
      List<ListViewItem> ListViewItems = new List<ListViewItem> ( DocCollection.CountDocuments() );
            
      MacroscopeSinglePercentageProgressForm ProgressForm = new MacroscopeSinglePercentageProgressForm ( this.MainForm );
      decimal Count = 0;
      decimal TotalDocs = ( decimal )DocCollection.CountDocuments();
      decimal MajorPercentage = ( ( decimal )100 / TotalDocs ) * Count;
      
      if( MacroscopePreferencesManager.GetShowProgressDialogues() )
      {

        ProgressForm.UpdatePercentages(
          Title: "Preparing Display",
          Message: "Processing document collection for display:",
          MajorPercentage: MajorPercentage,
          ProgressLabelMajor: string.Format( "Document {0} / {1}", Count, TotalDocs )
        );  

      }
                   
      foreach( MacroscopeDocument msDoc in DocCollection.IterateDocuments() )
      {

        Boolean bProceed = false;

        if( ( ( int )msDoc.GetStatusCode() >= 400 ) && ( ( int )msDoc.GetStatusCode() <= 499 ) )
        {
          bProceed = true;
        }
        else
        if( ( ( int )msDoc.GetStatusCode() >= 500 ) && ( ( int )msDoc.GetStatusCode() <= 599 ) )
        {
          bProceed = true;
        }

        if( bProceed )
        {
          this.RenderListView( 
            ListViewItems: ListViewItems,
            msDoc: msDoc,
            Url: msDoc.GetUrl()
          );
        }
        else
        {
          this.RemoveFromListView( Url: msDoc.GetUrl() );
        }

        if( MacroscopePreferencesManager.GetShowProgressDialogues() )
        {
                
          Count++;
          TotalDocs = ( decimal )DocCollection.CountDocuments();
          MajorPercentage = ( ( decimal )100 / TotalDocs ) * Count;
        
          ProgressForm.UpdatePercentages(
            Title: null,
            Message: null,
            MajorPercentage: MajorPercentage,
            ProgressLabelMajor: string.Format( "Document {0} / {1}", Count, TotalDocs )
          );
        
        }

      }
               
      this.lvListView.Items.AddRange( ListViewItems.ToArray() );
      
      if( MacroscopePreferencesManager.GetShowProgressDialogues() )
      {
        ProgressForm.DoClose();
      }
      
      ProgressForm.Dispose();

    }

    /**************************************************************************/

    protected override void RenderListView (
      List<ListViewItem> ListViewItems,
      MacroscopeDocument msDoc,
      string Url
    )
    {

      ListViewItem lvItem = null;
      string PairKey = Url;
      string StatusCode = ( ( int )msDoc.GetStatusCode() ).ToString();
      string Status = msDoc.GetStatusCode().ToString();

      if( this.lvListView.Items.ContainsKey( PairKey ) )
      {

        try
        {

          lvItem = this.lvListView.Items[ PairKey ];

          lvItem.SubItems[ 0 ].Text = Url;
          lvItem.SubItems[ 1 ].Text = StatusCode;
          lvItem.SubItems[ 2 ].Text = Status;
          lvItem.SubItems[ 3 ].Text = msDoc.GetErrorCondition();

        }
        catch( Exception ex )
        {
          this.DebugMsg( string.Format( "MacroscopeDisplayErrors 1: {0}", ex.Message ) );
        }

      }
      else
      {

        try
        {

          lvItem = new ListViewItem ( PairKey );
          lvItem.UseItemStyleForSubItems = false;
          lvItem.Name = PairKey;

          lvItem.SubItems[ 0 ].Text = Url;
          lvItem.SubItems.Add( StatusCode );
          lvItem.SubItems.Add( Status );
          lvItem.SubItems.Add( msDoc.GetErrorCondition() );

          ListViewItems.Add( lvItem );

        }
        catch( Exception ex )
        {
          this.DebugMsg( string.Format( "MacroscopeDisplayErrors 2: {0}", ex.Message ) );
        }

      }

      if( lvItem != null )
      {

        lvItem.ForeColor = Color.Blue;
        
        try
        {
          
          if( Regex.IsMatch( StatusCode, "^[2]" ) )
          {
            lvItem.SubItems[ 1 ].ForeColor = Color.Green;
            lvItem.SubItems[ 2 ].ForeColor = Color.Green;
          }
          else
          if( Regex.IsMatch( StatusCode, "^[3]" ) )
          {
            lvItem.SubItems[ 1 ].ForeColor = Color.Goldenrod;
            lvItem.SubItems[ 2 ].ForeColor = Color.Goldenrod;
          }
          else
          if( Regex.IsMatch( StatusCode, "^[45]" ) )
          {
            lvItem.SubItems[ 1 ].ForeColor = Color.Red;
            lvItem.SubItems[ 2 ].ForeColor = Color.Red;
          }
          else
          {
            lvItem.SubItems[ 1 ].ForeColor = Color.Blue;
            lvItem.SubItems[ 2 ].ForeColor = Color.Blue;
          }
          
        }
        catch( Exception ex )
        {
          this.DebugMsg( string.Format( "MacroscopeDisplayErrors 3: {0}", ex.Message ) );
        }
        
      }

    }

    /**************************************************************************/

    private void RemoveFromListView ( string Url )
    {

      string PairKey = Url;

      if( this.lvListView.Items.ContainsKey( PairKey ) )
      {

        this.lvListView.BeginUpdate();
        
        lock( this.lvListView.Items )
        {
          this.lvListView.Items.Remove( this.lvListView.Items[ PairKey ] );
        }
        
        this.lvListView.EndUpdate();

      }

    }

    /**************************************************************************/

    protected override void RenderUrlCount ()
    {
    }

    /**************************************************************************/

  }

}
