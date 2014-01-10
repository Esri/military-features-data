/*
 | Copyright 2013 Esri
 |
 | Licensed under the Apache License, Version 2.0 (the "License");
 | you may not use this file except in compliance with the License.
 | You may obtain a copy of the License at
 |
 |    http://www.apache.org/licenses/LICENSE-2.0
 |
 | Unless required by applicable law or agreed to in writing, software
 | distributed under the License is distributed on an "AS IS" BASIS,
 | WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 | See the License for the specific language governing permissions and
 | limitations under the License.
 */

import java.awt.image.BufferedImage;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Iterator;

import javax.imageio.ImageIO;

import com.almworks.sqlite4java.SQLiteConnection;
import com.almworks.sqlite4java.SQLiteException;
import com.almworks.sqlite4java.SQLiteStatement;
import com.esri.core.symbol.advanced.SymbolDictionary;
import com.esri.core.symbol.advanced.SymbolDictionary.DictionaryType;
import com.esri.runtime.ArcGISRuntime;

/**
 * A command line utility for exporting sets of ArcGIS Runtime Military/Dictionary Symbols
 * 
 * Usage: 
 * java -jar ExportSymbols.jar [(symbol name/id),"ALL"] [Type:("Point","Line","Area")]
 * java -jar ExportSymbols.jar [(symbol name/id),"ALL"] [Type:("Point" "Line" "Area")] {Standard:("2525", "APP6")} 
 * java -jar ExportSymbols.jar [(symbol name/id),"FILE"] [Filename with SIDCs/Names)] {Standard:("2525", "APP6")} 
 * 
 * Note: If using the file option, the file must be a comma-delimited file where the SIDC or Name is in the first column
 * 
 * Example:
 * If using version built with the provided ant build.xml:
 * java -classpath dist -jar dist/ExportSymbols.jar "SFGPUCI-----USG"
 * java -classpath dist -jar dist/ExportSymbols.jar ALL
 * java -classpath dist -jar dist/ExportSymbols.jar ALL POINT
 * java -classpath dist -jar dist/ExportSymbols.jar ALL LINE APP6
 * java -classpath dist -jar dist/ExportSymbols.jar FILE MyListOfSIDCs.txt
 */
public class ExportSymbols {

	/**
	 * @param args
	 */
	public static void main(String[] args) {

		try {
			ArcGISRuntime.initialize();
		} catch (Throwable t) {
			t.printStackTrace();
		}		
		
		String NOT_SET = "NOT SET";

		// Defaults used if no arguments given:		
		String sidc = "SFGPUCI-----USG";
		String standard = "2525";		
		String allFlag = NOT_SET;
		String geometryType = NOT_SET;
		String readFromFileName = NOT_SET;

		if (args.length == 0 || args[0].equals("help")) {
			Usage();
		} else if (args.length == 1) {			
			sidc = args[0];
			allFlag = args[0];
		} else if (args.length == 2) {						
			allFlag = args[0].toUpperCase();
			geometryType = args[1].toUpperCase();
			readFromFileName = args[1];
		} else if (args.length == 3) {						
			allFlag = args[0].toUpperCase();
			geometryType = args[1].toUpperCase();
			readFromFileName = args[1];
			standard = args[2].toUpperCase();
		} else {
			Usage();
			System.exit(0); 
		}

		try {				
			SymbolDictionary sd = null;
			
			if (standard.contains("APP"))
				sd = new SymbolDictionary(DictionaryType.App6B);
			else
				sd = new SymbolDictionary(DictionaryType.Mil2525C);

			System.out.println("Running with settings: " + allFlag + ":" + geometryType
					+ ":" + standard);		
			
			if (allFlag.equals("ALL")) {	
				if (geometryType.equals(NOT_SET))
					getAll(sd, "ALL", standard);
				else
					getAll(sd, geometryType, standard);
			}
			else if (allFlag.equals("FILE")) {
				
				if (readFromFileName.equals(NOT_SET))
					System.out.println("FILE option: input file not specified");
				else
					getFromFile(sd, readFromFileName);				
			}
			else { // No "ALL" "FILE" flag, just do a single one
				// export a single symbol name/id
				export(sd, sidc);				
			}

		} catch (Throwable t) {
			t.printStackTrace();
		}							
		
	}

	public static void Usage() {
        System.out.println("Usage: java -jar ExportSymbols.jar [(symbol name/id) or \"ALL\"] [Type:(\"Point\" \"Line\" \"Area\")] {Standard:(\"2525\", \"APP6\")} ");
	}
	
	public static boolean isValidSymbol(MilitarySymbol symbol) {
	
		if (symbol == null)
			return false;
		
		String symbolId = symbol.getSymbolId();
		
		if (symbolId.length() == 15) {
			System.out.println("Found: " + symbolId + ":" + symbol.getName() + ":" + symbol.getGeometryType());
			return true;
		}		
		else {
			System.out.println("Skipping: " + symbolId + ":" + symbol.getName() + ":" + symbol.getGeometryType() + ":SIDC Length=" + symbolId.length());
			return false;
		}
	}
	
	public static void getFromFile(SymbolDictionary sd, String fileName) {
		
		try {
			
			System.out.println("Reading symbols from file: " + fileName);
			
			File dataFile = new File(fileName);
	        if (!dataFile.exists()) {  
	            System.err.println ("ERROR: Could not find file: " + fileName); 
	            return;
	        } 
	        
	        BufferedReader reader = new BufferedReader(new FileReader(fileName));
	        String line = null;
	        while ((line = reader.readLine()) != null) {
	        	if (!line.startsWith("#")) { // Skip lines with "#"
	        		
	        		String[] columns = line.split(",");
	        		
	        		String nameOrSidc = columns[0];
	        		
	        		export(sd, nameOrSidc);	
	        	}
	        }       
        
		}
		catch (java.io.IOException ioEx) {
            System.err.println ("ERROR: exception while reading file: " + fileName); 			
		}
		
	}
	
	public static void getAll(SymbolDictionary sd, String geometryType, String standard)
			throws SQLiteException {
		System.out.println("Getting all symbols matching type: " + geometryType + 
				", in standard: " + standard);

		// Assumes will be run from: test\TestExportSymbolsAll
		String dbFileLocation = "../../data/mil2525c/dictionary/mil2525c.dat";

		if (standard.contains("APP"))
			dbFileLocation = "../../data/app6b/dictionary/app6b.dat";
			
		File dbFile = new File(dbFileLocation);
			
		if (!dbFile.exists()) {			
			// Just in case accidentally run from dist folder...
			dbFileLocation = "../../../data/mil2525c/dictionary/mil2525c.dat";
			dbFile = new File(dbFileLocation);
			
			// TODO: Just use the one from the Runtime install/deploy if we
			// can't find this expected version from the Github repo clone
			
			if (!dbFile.exists()) {
				System.out.println("Exiting: Can't find dictionary file at: " + dbFileLocation);
				System.exit(-1);		
			}
		}
			
		// Query all symbols
		SQLiteConnection db = new SQLiteConnection(dbFile);
		db.open(true);

		SQLiteStatement st = db
				.prepare("SELECT Name,SymbolId,GeometryType FROM SymbolInfo");

		ArrayList<MilitarySymbol> symbolList = new ArrayList<MilitarySymbol>();

		while (st.step()) {
			String dbName = st.columnString(0);
			String dbSidc = st.columnString(1);
			String dbGeometryType = st.columnString(2);
			
			MilitarySymbol dbSymbol = new MilitarySymbol(dbName, dbSidc, dbGeometryType);

			// if name, sdic is empty, do not add to the list
			if (!(dbName.equals("") || (dbSidc.equals("")))) {
				symbolList.add(dbSymbol);
			}
		}

		// Now export those that match (and are valid)
		Iterator<MilitarySymbol> iter = symbolList.iterator();
		while (iter.hasNext()) {

			MilitarySymbol symbol = iter.next();
			String symbolId = symbol.getSymbolId();
			
			if (geometryType.equals("ALL")) {
				
				if (isValidSymbol(symbol))
					export(sd, symbolId);
				
			} else if (geometryType.contains("POINT")
					&& symbol.getGeometryType().equals("P")) {
				
				if (isValidSymbol(symbol))
					export(sd, symbolId);

			} else if (geometryType.contains("LINE")
					&& symbol.getGeometryType().equals("L")) {
				
				if (isValidSymbol(symbol))
					export(sd, symbolId);

			} else if (geometryType.contains("AREA")
					&& symbol.getGeometryType().equals("A")) {

				if (isValidSymbol(symbol))
					export(sd, symbolId);
				
			}
		}

	}

	public static void export(SymbolDictionary sd, String sidc) {		

		String fixedSidc = sidc.replace('*', '-');
		
		BufferedImage image = sd.getSymbolImage(fixedSidc, 256, 256);
		File file = new File(fixedSidc + ".png");
		System.out.println("Exporting Symbol: " + fixedSidc);
		try {
			ImageIO.write(image, "png", file);
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

}
