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
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;

namespace SEOMacroscope
{

	/// <summary>
	/// Description of MacroscopeDisplayRedirectsAudit.
	/// </summary>

	public class MacroscopeDisplayRedirectsAudit : MacroscopeDisplayListView
	{

		/**************************************************************************/

		static Boolean ListViewConfigured = false;
		
		/**************************************************************************/

		public MacroscopeDisplayRedirectsAudit ( MacroscopeMainForm MainFormNew, ListView lvListViewNew )
			: base( MainFormNew, lvListViewNew )
		{

			MainForm = MainFormNew;
			lvListView = lvListViewNew;
						
			if( MainForm.InvokeRequired )
			{
				MainForm.Invoke(
					new MethodInvoker (
						delegate
						{
							ConfigureListView();
						}
					)
				);
			}
			else
			{
				ConfigureListView();
			}

		}

		/**************************************************************************/
		
		void ConfigureListView ()
		{
			if( !ListViewConfigured )
			{
				ListViewConfigured = true;
			}
		}

		/**************************************************************************/

		protected override void RenderListView ( MacroscopeDocument msDoc, string sUrl )
		{

			Boolean bProcess;

			if( msDoc.GetIsRedirect() )
			{
				bProcess = true;
			}
			else
			{
				bProcess = false;
			}

			if( bProcess )
			{

				string sOriginURL = msDoc.GetUrlRedirectFrom();
				string sStatusCode = msDoc.GetStatusCode().ToString();
				string sDestinationURL = msDoc.GetUrlRedirectTo();

				string sPairKey = string.Join( "", sUrl );

				if( 
					( sOriginURL != null )
					&& ( sOriginURL.Length > 0 )
					&& ( sStatusCode != null )
					&& ( sStatusCode.Length > 0 )
					&& ( sDestinationURL != null )
					&& ( sDestinationURL.Length > 0 ) )
				{
					
					ListViewItem lvItem = null;

					if( this.lvListView.Items.ContainsKey( sPairKey ) )
					{
							
						try
						{

							lvItem = this.lvListView.Items[ sPairKey ];
							lvItem.SubItems[ 0 ].Text = sUrl;
							lvItem.SubItems[ 1 ].Text = sStatusCode;
							lvItem.SubItems[ 2 ].Text = sOriginURL;
							lvItem.SubItems[ 3 ].Text = sDestinationURL;

						}
						catch( Exception ex )
						{
							DebugMsg( string.Format( "MacroscopeDisplayRedirectsAudit 1: {0}", ex.Message ) );
						}

					}
					else
					{
							
						try
						{

							lvItem = new ListViewItem ( sPairKey );
							lvItem.UseItemStyleForSubItems = false;
							
							lvItem.Name = sPairKey;

							lvItem.SubItems[ 0 ].Text = sUrl;
							lvItem.SubItems.Add( sStatusCode );
							lvItem.SubItems.Add( sOriginURL );
							lvItem.SubItems.Add( sDestinationURL );

							this.lvListView.Items.Add( lvItem );

						}
						catch( Exception ex )
						{
							DebugMsg( string.Format( "MacroscopeDisplayRedirectsAudit 2: {0}", ex.Message ) );
						}

					}

					if( lvItem != null )
					{

						if( !msDoc.GetIsExternal() )
						{
							
							for( int i = 0 ; i <= 3 ; i++ )
								lvItem.SubItems[ 0 ].ForeColor = Color.Blue;
								
							if( Regex.IsMatch( sStatusCode, "^[2]" ) )
							{
								for( int i = 0 ; i <= 3 ; i++ )
									lvItem.SubItems[ 0 ].ForeColor = Color.Green;
							}
							else
							if( Regex.IsMatch( sStatusCode, "^[3]" ) )
							{
								for( int i = 0 ; i <= 3 ; i++ )
									lvItem.SubItems[ 0 ].ForeColor = Color.Goldenrod;
							}
							else
							if( Regex.IsMatch( sStatusCode, "^[45]" ) )
							{
								for( int i = 0 ; i <= 3 ; i++ )
									lvItem.SubItems[ 0 ].ForeColor = Color.Red;
							}

						}
						else
						{
							for( int i = 0 ; i <= 3 ; i++ )
								lvItem.SubItems[ 0 ].ForeColor = Color.Gray;
						}

					}
					
				}

			}
			
		}

		/**************************************************************************/

	}

}