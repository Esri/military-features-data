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
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Iterator;

import javax.imageio.ImageIO;

import com.esri.core.symbol.advanced.SymbolDictionary;
import com.esri.core.symbol.advanced.SymbolDictionary.DictionaryType;
import com.esri.runtime.ArcGISRuntime;

/**
 * A command line utility for exporting ArcGIS Runtime Military/Dictionary Symbols
 * 
 * Usage: 
 * java -jar ExportSymbol.jar [symbol name/id] {Standard:[2525/APP6]}
 * 
 * Example:
 * If using version built with the provided ant build.xml:
 * java -classpath dist -jar dist/ExportSymbol.jar "SFGPUCI-----USG"
 * java -classpath dist -jar dist/ExportSymbol.jar "Infantry F" "APP6"
 */
public class ExportSymbol {

	/**
	 * @param args
	 */
	public static void main(String[] args) {

		try {
			ArcGISRuntime.initialize();
		} catch (Throwable t) {			
			t.printStackTrace();
		}

		// Defaults used if no arguments given:
		String sidc = "SFGPUCI-----USG";
		String standard = "2525";

		if (args.length == 0 || args[0].equals("help")) {
			Usage();
		} else if (args.length == 1) {			
			sidc = args[0];
		} else if (args.length == 2) {						
			sidc = args[0];
			standard = args[1];		
		}
		else {
			 Usage();
			 System.exit(0); 
		}

		try {
			// perform export			
			SymbolDictionary sd = null;
			
			if (standard.toUpperCase().contains("APP"))
				sd = new SymbolDictionary(DictionaryType.App6B);
			else
				sd = new SymbolDictionary(DictionaryType.Mil2525C);
							
			export(sd, sidc);

		} catch (Throwable t) {
			t.printStackTrace();
		}		
	}

	public static void Usage() {
            System.out.println("Usage: java -jar ExportSymbol.jar [symbol name/id] {Standard:[2525/APP6]}");
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
