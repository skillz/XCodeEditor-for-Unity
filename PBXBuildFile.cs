using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEditor.SKZXCodeEditor
{
	public class PBXBuildFile : PBXObject
	{
		private const string FILE_REF_KEY = "fileRef";
		private const string SETTINGS_KEY = "settings";
		private const string ATTRIBUTES_KEY = "ATTRIBUTES";
		private const string WEAK_VALUE = "Weak";
		private const string COMPILER_FLAGS_KEY = "COMPILER_FLAGS";
		private const string CODE_SIGN_VALUE = "CodeSignOnCopy";
		private const string REMOVE_HEADERS_VALUE = "RemoveHeadersOnCopy";
		
		public PBXBuildFile( PBXFileReference fileRef, bool weak = false, bool embed = false ) : base()
		{
			
			this.Add( FILE_REF_KEY, fileRef.guid );
			SetWeakLink( weak );
			SetEmbed( embed );

//    def Create(cls, file_ref, weak=False):
//        if isinstance(file_ref, PBXFileReference):
//            file_ref = file_ref.id
//
//        bf = cls()
//        bf.id = cls.GenerateId()
//        bf['fileRef'] = file_ref
//
//        if weak:
//            bf.set_weak_link(True)
//
//        return bf
		}
		
		public PBXBuildFile( string guid, PBXDictionary dictionary ) : base ( guid, dictionary )
		{
//			Debug.Log( "constructor child" );
		}

		public bool SetEmbed( bool embed = false )
		{
			PBXDictionary settings = null;
			PBXList attributes = null;
			
			if( !_data.ContainsKey( SETTINGS_KEY ) ) {
				if( embed ) {
					attributes = new PBXList();
					attributes.Add( CODE_SIGN_VALUE );
					attributes.Add( REMOVE_HEADERS_VALUE );

					settings = new PBXDictionary();
					settings.Add( ATTRIBUTES_KEY, attributes );
					_data[ SETTINGS_KEY ] = settings;
				}
				return true;
			}
			
			settings = _data[ SETTINGS_KEY ] as PBXDictionary;
			if( !settings.ContainsKey( ATTRIBUTES_KEY ) ) {
				if( embed ) {
					attributes = new PBXList();
					attributes.Add( CODE_SIGN_VALUE );
					attributes.Add( REMOVE_HEADERS_VALUE );
					settings.Add( ATTRIBUTES_KEY, attributes );
					return true;
				}
				else {
					return false;
				}
			}
			else {
				attributes = settings[ ATTRIBUTES_KEY ] as PBXList;
			}
			
			if( embed ) {
				attributes.Add( CODE_SIGN_VALUE );
				attributes.Add( REMOVE_HEADERS_VALUE );
			}
			/*
			 * No need to remove these
			 * else {
				attributes.Remove( WEAK_VALUE );
			}*/
			
			settings.Add( ATTRIBUTES_KEY, attributes );
			this.Add( SETTINGS_KEY, settings );
			
			return true;
		}

		public bool SetWeakLink( bool weak = false )
		{
			PBXDictionary settings = null;
			PBXList attributes = null;
			
			if( !_data.ContainsKey( SETTINGS_KEY ) ) {
				if( weak ) {
					attributes = new PBXList();
					attributes.Add( WEAK_VALUE );
					
					settings = new PBXDictionary();
					settings.Add( ATTRIBUTES_KEY, attributes );
					_data[ SETTINGS_KEY ] = settings;
				}
				return true;
			}
			
			settings = _data[ SETTINGS_KEY ] as PBXDictionary;
			if( !settings.ContainsKey( ATTRIBUTES_KEY ) ) {
				if( weak ) {
					attributes = new PBXList();
					attributes.Add( WEAK_VALUE );
					settings.Add( ATTRIBUTES_KEY, attributes );
					return true;
				}
				else {
					return false;
				}
			}
			else {
				attributes = settings[ ATTRIBUTES_KEY ] as PBXList;
			}
			
			if( weak ) {
				attributes.Add( WEAK_VALUE );
			}
			else {
				attributes.Remove( WEAK_VALUE );
			}
			
			settings.Add( ATTRIBUTES_KEY, attributes );
			this.Add( SETTINGS_KEY, settings );
			
			return true;
		}
		
		public bool AddCompilerFlag( string flag )
		{
			if( !_data.ContainsKey( SETTINGS_KEY ) )
				_data[ SETTINGS_KEY ] = new PBXDictionary();
			
			if( !((PBXDictionary)_data[ SETTINGS_KEY ]).ContainsKey( COMPILER_FLAGS_KEY ) ) {
				((PBXDictionary)_data[ SETTINGS_KEY ]).Add( COMPILER_FLAGS_KEY, flag );
				return true;
			}
			
			string[] flags = ((string)((PBXDictionary)_data[ SETTINGS_KEY ])[ COMPILER_FLAGS_KEY ]).Split( ' ' );
			foreach( string item in flags ) {
				if( item.CompareTo( flag ) == 0 )
					return false;
			}
			
			((PBXDictionary)_data[ SETTINGS_KEY ])[ COMPILER_FLAGS_KEY ] = ( string.Join( " ", flags ) + " " + flag );
			return true;
			
//		def add_compiler_flag(self, flag):
//        k_settings = 'settings'
//        k_attributes = 'COMPILER_FLAGS'
//
//        if not self.has_key(k_settings):
//            self[k_settings] = PBXDict()
//
//        if not self[k_settings].has_key(k_attributes):
//            self[k_settings][k_attributes] = flag
//            return True
//
//        flags = self[k_settings][k_attributes].split(' ')
//
//        if flag in flags:
//            return False
//
//        flags.append(flag)
//
//        self[k_settings][k_attributes] = ' '.join(flags)
		}
		
	}
}
