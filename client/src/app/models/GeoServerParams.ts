export class GeoServerParams {
    service = 'wfs';
    version = '1.0.0';
    request = 'GetFeature';
    srsName = 'EPSG:4326';
    outputFormat = 'json';
    typeNamesTrafficAccidents = 'TrafficAccidentsApp:AllNotDeleted';
    typeNamesSettlements = 'TrafficAccidentsApp:settlements';
    typeNamesMunicipalities = 'TrafficAccidentsApp:municipalities';
    typeNamesCities = 'TrafficAccidentsApp:cities';
    typeNamesWithCategoryFilters = 'TrafficAccidentsApp:WithCategoryFilters';
}