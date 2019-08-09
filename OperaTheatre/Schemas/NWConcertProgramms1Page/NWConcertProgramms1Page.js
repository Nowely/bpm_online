define("NWConcertProgramms1Page", ["ProcessModuleUtilities"], function(ProcessModuleUtilities) {
	return {
		entitySchemaName: "NWConcertProgramms",
		attributes: {
			"maxNumberOfProgramm": {
				dataValueType: Terrasoft.DataValueType.INTEGER,
				value: -1
			},
			"oldValueOfPeriodicity": {
				dataValueType: Terrasoft.DataValueType.TEXT,
				value: ""
			},
			"oldValueOfisActive": {
				dataValueType: Terrasoft.DataValueType.BOOLEAN,
				value: false
			}
		},
		modules: /**SCHEMA_MODULES*/{}/**SCHEMA_MODULES*/,
		details: /**SCHEMA_DETAILS*/{
			"Files": {
				"schemaName": "FileDetailV2",
				"entitySchemaName": "NWConcertProgrammsFile",
				"filter": {
					"masterColumn": "Id",
					"detailColumn": "NWConcertProgramms"
				}
			},
			"NWSchema3Detailf5e0971f": {
				"schemaName": "NWSchema3Detail",
				"entitySchemaName": "NWPerformance",
				"filter": {
					"detailColumn": "NWConcertProgram",
					"masterColumn": "Id"
				}
			}
		}/**SCHEMA_DETAILS*/,
		businessRules: /**SCHEMA_BUSINESS_RULES*/{}/**SCHEMA_BUSINESS_RULES*/,
		methods: {
			getActions: function() {
				var parentActions =  this.callParent(arguments);
				parentActions.addItem(this.getButtonMenuItem({
                    Type: "Terrasoft.MenuSeparator",
                    Caption: ""
                }));
				parentActions.addItem(this.getButtonMenuItem({
					"Click": {"bindTo": "callAddDetailsProcess"},
					"Visible": true,
					"Caption": {"bindTo": "Resources.Strings.AddRecordsInDetail"},
					"Enabled": true,
				}));
				
				return parentActions;
			},
			callAddDetailsProcess: function() {
                var args = {
                    sysProcessName: "NWAddRecordsInDetail",
                    parameters: {
                        recordId: this.$Id
                    }
                };
                ProcessModuleUtilities.executeProcess(args);
            },
			subscriptionFunction: function() {
				Terrasoft.ServerChannel.on(Terrasoft.EventName.ON_MESSAGE,
				this.bpListenerMessage, this);
			},
				bpListenerMessage: function(scope, message) {
					if (message.Body === "Update"){
						this.updateDetails();
					}
			},
			CheckNumberOfEveryDaysProgramms: function (parentSave, parentArguments)
			{
				var everyDayId = "646a3709-71ad-49bb-bb47-93d27338e226";
				
				if ((this.get("NWPeriodicity").value !== everyDayId) 
				|| (this.get("NWisActive") !== true) 
				|| ((this.get("NWPeriodicity").value === this.get("oldValueOfPeriodicity")) 
				&& (this.get("oldValueOfisActive") === true))) { 
					parentSave.apply(this, parentArguments); 
					this.updateOldValues();
				}
				else {
					this.Terrasoft.SysSettings.querySysSettingsItem("maxNumberOfProgramm",
    			   function(value) { this.set("maxNumberOfProgramm", value); }, this);    //     		
				
				var proggramsRecords = Ext.create("Terrasoft.EntitySchemaQuery", {
					rootSchemaName: "NWConcertProgramms"});
				var filterGroup = Ext.create("Terrasoft.FilterGroup");
				var esqFirstFilter = proggramsRecords.createColumnFilterWithParameter(Terrasoft.ComparisonType.EQUAL,
														"NWPeriodicity",
														"646A3709-71AD-49BB-BB47-93D27338E226");
				filterGroup.addItem(esqFirstFilter);
				var esqSecondFilter = proggramsRecords.createColumnFilterWithParameter(Terrasoft.ComparisonType.EQUAL,
														"NWisActive",
														1);
				filterGroup.addItem(esqSecondFilter);
				proggramsRecords.filters.add("firstFilter", filterGroup);
				proggramsRecords.getEntityCollection(function(result) {
				if (result.collection.collection.length >= this.get("maxNumberOfProgramm"))
				{
					let myCaption = "Свободных конертных залов мало и допускается не более " + 
								this.get("maxNumberOfProgramm") + 
								" программ.";
					this.showInformationDialog(myCaption);
				}
				else 
				{
					parentSave.apply(this, parentArguments);
					this.updateOldValues();
				}
				
				}, this);
				}
			},
			updateOldValues: function() {
				this.set("oldValueOfPeriodicity", this.get("NWPeriodicity").value);
				this.set("oldValueOfisActive", this.get("NWisActive"));
			},
			init: function() {
				this.callParent(arguments);
				this.subscriptionFunction();
			},
			onEntityInitialized: function() {
				this.callParent(arguments);
				if (this.get("IsEditable")){
				this.updateOldValues();
				}
			},
			getParentMethod: function() {
			var method,
				superMethod = (method = this.getParentMethod.caller) && (method.$previous ||
					((method = method.$owner ? method : method.caller) &&
					method.$owner.superclass[method.$name]));
			return superMethod;
			},
			save: function() {
				var parentSave = this.getParentMethod();
				var parentArguments = arguments;
				this.CheckNumberOfEveryDaysProgramms(parentSave, parentArguments);
			},
		},
		dataModels: /**SCHEMA_DATA_MODELS*/{}/**SCHEMA_DATA_MODELS*/,
		diff: /**SCHEMA_DIFF*/[
			{
				"operation": "insert",
				"name": "NWName0f8b31b8-a57a-48f6-863e-526f81b4167c",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 1,
						"column": 0,
						"row": 0,
						"layoutName": "ProfileContainer"
					},
					"bindTo": "NWName"
				},
				"parentName": "ProfileContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "CreatedBy9829308f-8218-46e7-ba97-349deb3835c1",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 1,
						"column": 0,
						"row": 1,
						"layoutName": "ProfileContainer"
					},
					"bindTo": "CreatedBy"
				},
				"parentName": "ProfileContainer",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "Tabc3ae2a6bTabLabel",
				"values": {
					"caption": {
						"bindTo": "Resources.Strings.Tabc3ae2a6bTabLabelTabCaption"
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
				"name": "Tabc3ae2a6bTabLabelGroup0b257358",
				"values": {
					"caption": {
						"bindTo": "Resources.Strings.Tabc3ae2a6bTabLabelGroup0b257358GroupCaption"
					},
					"itemType": 15,
					"markerValue": "added-group",
					"items": []
				},
				"parentName": "Tabc3ae2a6bTabLabel",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Tabc3ae2a6bTabLabelGridLayout3d5217ff",
				"values": {
					"itemType": 0,
					"items": []
				},
				"parentName": "Tabc3ae2a6bTabLabelGroup0b257358",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "LOOKUPbf463164-772e-4c6e-b3f8-09c904ec0930",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 1,
						"column": 0,
						"row": 0,
						"layoutName": "Tabc3ae2a6bTabLabelGridLayout3d5217ff"
					},
					"bindTo": "NWCollective",
					"enabled": true,
					"contentType": 5
				},
				"parentName": "Tabc3ae2a6bTabLabelGridLayout3d5217ff",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "LOOKUP9a5fc88b-5419-44ac-98f9-1e19c1ec0928",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 0,
						"row": 1,
						"layoutName": "Tabc3ae2a6bTabLabelGridLayout3d5217ff"
					},
					"bindTo": "NWPeriodicity",
					"enabled": true,
					"contentType": 3
				},
				"parentName": "Tabc3ae2a6bTabLabelGridLayout3d5217ff",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "BOOLEANd7fd67f0-9bcc-4ba2-a5e8-08c6274f028b",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 12,
						"row": 1,
						"layoutName": "Tabc3ae2a6bTabLabelGridLayout3d5217ff"
					},
					"bindTo": "NWisActive",
					"enabled": true
				},
				"parentName": "Tabc3ae2a6bTabLabelGridLayout3d5217ff",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "INTEGERf8f99cc5-2f45-467c-91f4-b24f286cde89",
				"values": {
					"layout": {
						"colSpan": 11,
						"rowSpan": 1,
						"column": 0,
						"row": 2,
						"layoutName": "Tabc3ae2a6bTabLabelGridLayout3d5217ff"
					},
					"bindTo": "NWCode",
					"enabled": true
				},
				"parentName": "Tabc3ae2a6bTabLabelGridLayout3d5217ff",
				"propertyName": "items",
				"index": 3
			},
			{
				"operation": "insert",
				"name": "STRINGa827aa06-7407-4100-ad6d-87992cbd6653",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 2,
						"column": 0,
						"row": 3,
						"layoutName": "Tabc3ae2a6bTabLabelGridLayout3d5217ff"
					},
					"bindTo": "NWComment",
					"enabled": true,
					"contentType": 0
				},
				"parentName": "Tabc3ae2a6bTabLabelGridLayout3d5217ff",
				"propertyName": "items",
				"index": 4
			},
			{
				"operation": "insert",
				"name": "NWSchema3Detailf5e0971f",
				"values": {
					"itemType": 2,
					"markerValue": "added-detail"
				},
				"parentName": "Tabc3ae2a6bTabLabel",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "NotesAndFilesTab",
				"values": {
					"caption": {
						"bindTo": "Resources.Strings.NotesAndFilesTabCaption"
					},
					"items": [],
					"order": 1
				},
				"parentName": "Tabs",
				"propertyName": "tabs",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "Files",
				"values": {
					"itemType": 2
				},
				"parentName": "NotesAndFilesTab",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "NotesControlGroup",
				"values": {
					"itemType": 15,
					"caption": {
						"bindTo": "Resources.Strings.NotesGroupCaption"
					},
					"items": []
				},
				"parentName": "NotesAndFilesTab",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "Notes",
				"values": {
					"bindTo": "NWNotes",
					"dataValueType": 1,
					"contentType": 4,
					"layout": {
						"column": 0,
						"row": 0,
						"colSpan": 24
					},
					"labelConfig": {
						"visible": false
					},
					"controlConfig": {
						"imageLoaded": {
							"bindTo": "insertImagesToNotes"
						},
						"images": {
							"bindTo": "NotesImagesCollection"
						}
					}
				},
				"parentName": "NotesControlGroup",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "merge",
				"name": "ESNTab",
				"values": {
					"order": 2
				}
			}
		]/**SCHEMA_DIFF*/
	};
});
