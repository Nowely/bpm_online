define("NWNWPerformance1Page", [], function() {
	return {
		entitySchemaName: "NWPerformance",
		attributes: {},
		modules: /**SCHEMA_MODULES*/{}/**SCHEMA_MODULES*/,
		details: /**SCHEMA_DETAILS*/{}/**SCHEMA_DETAILS*/,
		businessRules: /**SCHEMA_BUSINESS_RULES*/{}/**SCHEMA_BUSINESS_RULES*/,
		methods: {},
		dataModels: /**SCHEMA_DATA_MODELS*/{}/**SCHEMA_DATA_MODELS*/,
		diff: /**SCHEMA_DIFF*/[
			{
				"operation": "insert",
				"name": "Tabcbc1263aTabLabel",
				"values": {
					"caption": {
						"bindTo": "Resources.Strings.Tabcbc1263aTabLabelTabCaption"
					},
					"items": [],
					"order": 0
				},
				"parentName": "Tabs",
				"propertyName": "tabs",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Tabcbc1263aTabLabelGroupb83b4b20",
				"values": {
					"caption": {
						"bindTo": "Resources.Strings.Tabcbc1263aTabLabelGroupb83b4b20GroupCaption"
					},
					"itemType": 15,
					"markerValue": "added-group",
					"items": []
				},
				"parentName": "Tabcbc1263aTabLabel",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Tabcbc1263aTabLabelGridLayout7903ee6e",
				"values": {
					"itemType": 0,
					"items": []
				},
				"parentName": "Tabcbc1263aTabLabelGroupb83b4b20",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "NWConcertPrograma01b9a9d-e8ba-458c-9b7b-2edfc076e128",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 1,
						"column": 0,
						"row": 0,
						"layoutName": "Tabcbc1263aTabLabelGridLayout7903ee6e"
					},
					"bindTo": "NWConcertProgram"
				},
				"parentName": "Tabcbc1263aTabLabelGridLayout7903ee6e",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "NWDateOfPerformanced2e01ed3-0ed2-4986-9f00-df7f6426a4f9",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 0,
						"row": 1,
						"layoutName": "Tabcbc1263aTabLabelGridLayout7903ee6e"
					},
					"bindTo": "NWDateOfPerformance"
				},
				"parentName": "Tabcbc1263aTabLabelGridLayout7903ee6e",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "NWResponsible32174da1-1f03-4a06-aaff-8a7368b260dd",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 12,
						"row": 2,
						"layoutName": "Tabcbc1263aTabLabelGridLayout7903ee6e"
					},
					"bindTo": "NWResponsible"
				},
				"parentName": "Tabcbc1263aTabLabelGridLayout7903ee6e",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "NWPerformanceStates6019af54-778d-497b-9c01-9f8f55b70c02",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 12,
						"row": 1,
						"layoutName": "Tabcbc1263aTabLabelGridLayout7903ee6e"
					},
					"bindTo": "NWPerformanceStates"
				},
				"parentName": "Tabcbc1263aTabLabelGridLayout7903ee6e",
				"propertyName": "items",
				"index": 3
			},
			{
				"operation": "insert",
				"name": "NWNumberOfTickets7172a667-e6d8-414c-84e1-a548e7bd7859",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 0,
						"row": 2,
						"layoutName": "Tabcbc1263aTabLabelGridLayout7903ee6e"
					},
					"bindTo": "NWNumberOfTickets"
				},
				"parentName": "Tabcbc1263aTabLabelGridLayout7903ee6e",
				"propertyName": "items",
				"index": 4
			}
		]/**SCHEMA_DIFF*/
	};
});
