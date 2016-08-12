QT += core testlib

# QT -= gui

CONFIG += c++11 arcgis_runtime_qml_cpp100_0_0

TARGET = ExportSymbol
CONFIG += console
CONFIG -= app_bundle

TEMPLATE = app

SOURCES += main.cpp \
    symbolexporter.cpp

win32 {
    LIBS += \
        Ole32.lib
}

HEADERS += \
    symbolexporter.h
