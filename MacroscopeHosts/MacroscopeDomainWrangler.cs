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

namespace SEOMacroscope
{

	/// <summary>
	/// Description of MacroscopeDomainWrangler.
	/// </summary>

	public class MacroscopeDomainWrangler : Macroscope
	{

		/**************************************************************************/

		int Tolerance = 2;
		
		/**************************************************************************/
				
		public MacroscopeDomainWrangler ()
		{
		}

		/**************************************************************************/

		public Boolean IsWithinSameDomain ( string sDomainLeft, string sDomainRight )
		{
			return( this.IsWithinSameDomain( sDomainLeft, sDomainRight, this.Tolerance ) );
		}
		
		/**************************************************************************/
		
		public Boolean IsWithinSameDomain ( string sDomainLeft, string sDomainRight, int iTolerance )
		{

			Boolean bIsWithinSameDomain = false;
			int iScore = 0;
			
			string[] DomainShort;
			string[] DomainLong;

			string sDomainLeftReversed = this.ReverseString( sDomainLeft );
			string sDomainRightReversed = this.ReverseString( sDomainRight );

			int iScoreThreshold = this.Tolerance;
				
			if( iTolerance > 0 ) {
				iScoreThreshold = iTolerance;
			}

			if( sDomainLeftReversed.Length > sDomainRightReversed.Length ) {
				DomainShort = sDomainRightReversed.Split( '.' );
				DomainLong = sDomainLeftReversed.Split( '.' );
			} else {
				DomainShort = sDomainLeftReversed.Split( '.' );
				DomainLong = sDomainRightReversed.Split( '.' );
			}

			DomainShort = this.ReverseStrings( DomainShort );
			DomainLong = this.ReverseStrings( DomainLong );

			DebugMsg( string.Format( "DomainShort: {0} :: {1}", DomainShort.Length, string.Join( " ", DomainShort ) ) );
			DebugMsg( string.Format( "DomainLong: {0} :: {1}", DomainLong.Length, string.Join( " ", DomainLong ) ) );

			DebugMsg( "" );
						
			for( int i = 0; i < DomainShort.Length; i++ ) {

				if( DomainShort[ i ] == DomainLong[ i ] ) {
					iScore++;
				} else {
					break;
				}

				DebugMsg( string.Format( "MATCH CHECK: {0} :: {1} :: {2} :: {3}", i, bIsWithinSameDomain, DomainShort[ i ], DomainLong[ i ] ) );

			}

			DebugMsg( string.Format( "SCORE: {0}", iScore ) );

			if( DomainShort.Length >= 3 ) {
				if( iScore >= iScoreThreshold ) {
					bIsWithinSameDomain = true;
				}
			}

			DebugMsg( string.Format( "bIsWithinSameDomain: {0}", bIsWithinSameDomain ) );

			DebugMsg( "" );

			return( bIsWithinSameDomain );
		
		}

		/**************************************************************************/
		
		string ReverseString ( string sInput )
		{
			string sOutput = "";
			for( int i = ( sInput.Length - 1 ); i >= 0; i-- ) {
				sOutput += sInput[ i ];
			}
			return( sOutput );
		}
		
		/**************************************************************************/
				
		string[] ReverseStrings ( string[] sInput )
		{
			string[] sOutput = new string[sInput.Length];
			for( int i = 0; i < sInput.Length; i++ ) {
				sOutput[ i ] = this.ReverseString( sInput[ i ] );
			}
			return( sOutput );
		}

		/**************************************************************************/
		
	}

}
