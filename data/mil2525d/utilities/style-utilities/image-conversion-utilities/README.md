# military-features-data / data / mil2525d / utilities / style-utilities / image-conversion-utilities

## Purpose

* Converts a set of Scalable Vector Graphics (.svg) files (arranged in a directory tree) to other formats (ex: .png/.emf)
* Recreates the original folder structure/tree of the source folder in the destination folder

## Important Notes/Caveats/Limitations

* Windows-only
    * These utilities were written as DOS batch (.bat) files to be able to run relatively easily (with minimum configuration) i.e. without needing to install additional software (besides the .svg converter)
* IMPORTANT: No spaces in any of the paths (path to Inkscape, path to SVGs, etc.)
    * Because of the Windows batch file constraints (mainly needing to replace text in variables in a DOS batch file), there can't be any spaces in the paths
    * So, for instance, **don't** install your converter to **"Program Files"** or put the files to convert in **"My Documents"**
* Need to install a third party .svg converter utility, **we recommend Inkscape version 048.5.R10040.**
    * While several other .svg converters are available, it is HIGHLY recommended to use the specific version of Inkscape mentioned, as it is the only version verified to not distort the colors/fonts/dash pattern during this process. 

## Instructions 

* Ensure that an appropriate .svg converter is installed
    * This utility has been tested with Inkscape version 048.5.R10040. While several other .svg converters are available, it is highly recommended to use the version of inkscape mentioned below, as it is the only version verified to NOT distort the colors/fonts/dash pattern during this process
    * Inkscape is available at: http://www.inkscape.org/en/download/ (you may use 32 or 64-bit Windows version)
    * It is recommended that you use the [.7zip version](https://inkscape.org/en/gallery/item/3932/Inkscape-0.91-1-win32.7z) which allows you to extract anywhere (so you don't put a space in the path - [see limitations](#important-notescaveatslimitations) )
* Select the desired .bat that matches your desired conversion method
    * `ConvertTree-SVGtoEMF.bat` used to convert an .svg file tree to Enhanced Metafile Format (.emf) is the version you will normally want to use with this process.
* IMPORTANT: Modify the converter .bat command file to match your local paths
    *  Open and edit the desired .bat file, find the "IMPORTANT/TODO" section at the top, and edit the paths:
    *  (1) Converter location: full path to .svg converter
    *  (2) Source .svg file Root Folder
    *  (3) Desired Destination Folder
    *  See [sample converter .bat file for more information](./ConvertTree-SVGtoEMF.bat)
*  Run the desired .bat file
    *  `cd {local-path-to-bat}`
    *  Run the .bat, e.g. ConvertTree-SVGtoEMF.bat
    *  If you wish to capture the command-line output for later analysis of errors add the following to the command above: `ConverterOutput.txt 2>&1`
        *  If you wish to capture the output, alternately, you can run the file: `ConvertTree-SVGtoEMF-CaptureOutput.bat`
    *  (Full Example)  `ConvertTree-SVGtoEMF.bat > ConverterOutput.txt 2>&1` 
    *  *(May take several minutes depending on the number of files)*
    *  Check the output for errors
*  Check the destination folder for the converted image files
    * IMPORTANT NOTE: MS Paint (which is the default file association) does not support the latest version of EMF so you will need to use an alternative program to view the resulting EMF files (ex. MS Word)

## Alternative Workflow using Adobe Illustrator Actions

* If you have access to Adobe Illustrator (We are using CS6), you can use Actions in Batch Mode to convert your files from .svg to .emf.
* IMPORTANT: This process will export .emfs to one singular folder, not the origin .svg file directory.
* To start, download the `BatchConvertSVGtoEMF.aia` action set file contained in this repository.
* Open Adobe Illustrator, navigate to the Window pane, and open up Actions.
* Choose Load Actions from the Actions Panel menu. Navigate to the ExportasEMF action set, and click Open. 
* Under the `ExportasEMF` action, double click on the destination file path and edit it to ensure that your files will be converted to .emf in the desired location.
* Click on the `ExportasEMF action` within the action set. Then, navigate to the Actions Panel menu on the top right of the pane and choose Batch. This will open up the Batch pane.
    * Set the source folder to where your .svg files are located.
    * Make sure that the `Include All Subdirectories` check box is checked if your .svg files are stored in subdirectories.
    * Set the destination to `Save and Close.` You can specify the output folder by choosing `Override Action 'Export' Commands.`
    * Click OK, which will start the batch conversion process.
* Check for any unwanted converted .emf files in your output folder before moving on to `style-file-utilities`. 
* At this point, you will have to either re-map the svg files to their original directories, or edit the `Military-All-Icons.csv` to collapse the `filePath` column so that the paths reflect the singular output .emf folder. 
    * For example, you could replace "Appendices\Activities" with "" and do the same for the other appendices. For the Legacy folder, however, this may not work as well due to repeat entries in the .csv. As a result, it would be best to run this process separately for the legacy subdirectories. 