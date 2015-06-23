module LASI.documentViewer {
    'use strict';

    interface IDocumentController {
        title: string;
        processDocument: (documentId: string) => IDocumentModel;
        activate: () => void;
    }

    class DocumentController implements IDocumentController {
        title: string = 'DocumentController';
        private documentModel: IDocumentModel;
        static $inject = ['DocumentModelService', '$location'];

        constructor(private documentModelService: IDocumentModelService, private $location: ng.ILocationService) {
            this.activate();
        }
        processDocument(documentId: string) {
            return this.documentModelService.processDocument(documentId);
        }
        activate() {
            //this.documentModel = this.documentModelService.getData();
        }
    }

    angular
        .module(LASI.documentViewer.moduleName)
        .controller('DocumentController', DocumentController);
}