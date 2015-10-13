'use strict';
var DocumentController = (function () {
    function DocumentController($q, documentModelService) {
        this.$q = $q;
        this.documentModelService = documentModelService;
    }
    DocumentController.prototype.processDocument = function (id) {
        if (this.documentModel.id !== id) {
            return this.documentModelService.processDocument(id);
        }
        else {
            return this.$q.reject(this.documentModel);
        }
    };
    DocumentController.$inject = ['$q', 'MockDocumentModelService'];
    return DocumentController;
})();
exports.DocumentController = DocumentController;