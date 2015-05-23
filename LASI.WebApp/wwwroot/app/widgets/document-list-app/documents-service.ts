﻿(function() {
    'use strict';

    angular
        .module(LASI.documentList.ngName)
        .service('DocumentsService', DocumentsService);

    DocumentsService.$inject = ['$resource'];

    function DocumentsService($resource: ng.resource.IResourceService) {
        var userDocouments = $resource<IDocumentListItem[]>('api/UserDocuments/:id');
        this.getbyId = function(id) {
            return userDocouments.get({ id: id });
        };
        this.deleteById = function(id) {
            return userDocouments.delete({ id: id });
        };
    }
})();