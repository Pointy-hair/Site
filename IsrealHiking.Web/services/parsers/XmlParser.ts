﻿module IsraelHiking.Services.Parsers {

    declare var toGeoJSON: Function[];
    
    export class XmlParser extends BaseParser implements IParser {

        protected parseToGeoJson(content: string): GeoJSON.FeatureCollection {
            var document = (new DOMParser()).parseFromString(content, "text/xml");
            return toGeoJSON[this.getFormat()](document);
        }

        public toString(data: Common.DataContainer): string {
            var geoJson = super.toGeoJson(data);
            return this.convertToXml(geoJson);
        }

        // should be implemented in derived class 
        protected getFormat(): string { return ""; }

        // should be implemented in derived class 
        protected convertToXml(geoJson: GeoJSON.FeatureCollection): string { return ""; }
    }
}   