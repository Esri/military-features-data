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

/**
 * Encapsulates a dictionary entry
 */
public class MilitarySymbol {
	
	private String name;
	private String symbolId;
	private String geometryType;	
	
	public MilitarySymbol(String name, String symbolId, String geometryType)
	{
		setName(name);
		setSymbolId(symbolId);
		setGeometryType(geometryType);
	}
	
	public void setName(String name){
		this.name = name;
	}

	public void setSymbolId(String symbolId){
		this.symbolId = symbolId;
	}
	
	public void setGeometryType(String geom){
		this.geometryType = geom;
	}
	
	public String getName(){
		return name;
	}
	
	public String getSymbolId(){
		return symbolId;
	}
	
	public String getGeometryType(){
		return geometryType;
	}
}
