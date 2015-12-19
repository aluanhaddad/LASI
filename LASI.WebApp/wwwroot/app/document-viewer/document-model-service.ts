﻿'use strict';

documentModelService.$inject = ['$q', '$http'];

export function documentModelService($q: ng.IQService, $http: angular.IHttpService): DocumentModelService {
    return {
        processDocument(documentId) {
            var deferred = $q.defer();
            $http.get(`Analysis/${documentId}`, { cache: false }).then(d=> deferred.resolve(d.data));
            return deferred.promise;
        }
    };
}