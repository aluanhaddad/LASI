'use strict';
var DocumentListServiceProvider = (function () {
    function DocumentListServiceProvider() {
        this.$inject = ['$resource'];
        this.$get.$inject = ['$resource'];
    }
    DocumentListServiceProvider.prototype.setDocumentListUrl = function (url) {
        this.documentListUrl = url;
        return this;
    };
    DocumentListServiceProvider.prototype.setRecentDocumentCount = function (count) {
        this.recentDocumentCount = count;
        return this;
    };
    /**
     * @param $resource an instance of the Resource Service supplied by the angular-resource module.
     */
    DocumentListServiceProvider.prototype.$get = function ($resource) {
        var resource = $resource(this.documentListUrl + '?limit=' + this.recentDocumentCount, {}, {
            get: {
                method: 'GET',
                isArray: true
            },
            delete: {
                method: 'DELETE',
                isArray: false
            }
        });
        return {
            deleteDocument: function (documentId) {
                return resource.delete({ documentId: documentId })[0];
            },
            get: resource.get
        };
    };
    ;
    return DocumentListServiceProvider;
})();
exports.DocumentListServiceProvider = DocumentListServiceProvider;