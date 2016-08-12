TARGET = ValidateSymbols
CONFIG += console

QT += core gui sql

greaterThan(QT_MAJOR_VERSION, 4) {
    QT += widgets
}

# Important: requires file: $qtsdk\mkspecs\features\esri_runtime_qt_10_2_2.prf
# See ArcGIS Runtime Qt SDK documentation for more information
CONFIG += c++11 esri_runtime_qt_10_2_2

win32:CONFIG += \
  embed_manifest_exe

SOURCES += \
	main.cpp \
	imagehistogram.cpp \ 
    SymbolExporter.cpp \
    imagecomparer.cpp

HEADERS += \ 
	imagehistogram.h \ 
    SymbolExporter.h \
    imagecomparer.h

