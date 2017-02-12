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
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SEOMacroscope
{

	/// <summary>
	/// Description of MacroscopeColumnSorter.
	/// </summary>

	public class MacroscopeColumnSorter : IComparer
	{

		/**************************************************************************/

		int ColumnToSort;
		SortOrder OrderOfSort;
		CaseInsensitiveComparer ObjectCompare;

		/**************************************************************************/
				
		public MacroscopeColumnSorter ()
		{
			ColumnToSort = 0;
			OrderOfSort = SortOrder.None;
			ObjectCompare = new CaseInsensitiveComparer ();
		}

		/**************************************************************************/

		public int Compare ( object x, object y )
		{
			
			int compareResult;
			ListViewItem listviewX, listviewY;
			
			listviewX = ( ListViewItem )x;
			listviewY = ( ListViewItem )y;

			object [] ObjectPair = DetermineValueType(
				                       listviewX.SubItems[ ColumnToSort ].Text,
				                       listviewY.SubItems[ ColumnToSort ].Text
			                       );
			
			compareResult = ObjectCompare.Compare( ObjectPair[ 0 ], ObjectPair[ 1 ] );

			if( OrderOfSort == SortOrder.Ascending )
			{
				return compareResult;
			}
			else
			if( OrderOfSort == SortOrder.Descending )
			{
				return ( -compareResult );
			}
			else
			{
				return 0;
			}
			
		}
		
		/**************************************************************************/

		public int SortColumn
		{
			set
			{
				ColumnToSort = value;
			}
			get
			{
				return ColumnToSort;
			}
		}

		/**************************************************************************/
		
		public SortOrder Order
		{
			set
			{
				OrderOfSort = value;
			}
			get
			{
				return OrderOfSort;
			}
		}
	
		/**************************************************************************/
		
		object[] DetermineValueType ( string sTextX, string sTextY )
		{
			object [] ObjectPair = new object[2];
	
			ObjectPair[ 0 ] = sTextX;
			ObjectPair[ 1 ] = sTextY;

			if(
				Regex.IsMatch( sTextX, "^[0-9]+$" )
				&& Regex.IsMatch( sTextY, "^[0-9]+$" ) )
			{
				decimal DecimalX = decimal.Parse( sTextX );
				decimal DecimalY = decimal.Parse( sTextY );
				ObjectPair[ 0 ] = DecimalX;
				ObjectPair[ 1 ] = DecimalY;
			}
			
			if(
				Regex.IsMatch( sTextX, "^[0-9]+\\.[0-9]+$" )
				&& Regex.IsMatch( sTextY, "^[0-9]+\\.[0-9]+$" ) )
			{
				decimal DecimalX = decimal.Parse( sTextX );
				decimal DecimalY = decimal.Parse( sTextY );
				ObjectPair[ 0 ] = DecimalX;
				ObjectPair[ 1 ] = DecimalY;
			}

			// TODO: Add dates, etc.
			
			return( ObjectPair );
		}
		
		/**************************************************************************/

	}
	
}
