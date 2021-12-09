
(function () {
    'use strict';
    angular.module('FrontApp')
        .factory('BlogsFactory', function ($http) {

            return {                

                SearchBlogs: function (frontBlogRequest) {
                    return $http({
                        url: "/Blogs/SearchBlogs",
                        method: "POST",
                        data: { p_FrontBlogRequest: frontBlogRequest }
                    });
                },
                GetBlogCommentsByBlogId: function (BlogId) {
                    return $http({
                        url: "/Blogs/GetBlogCommentsByBlogId",
                        method: "POST",
                        data: { BlogId: BlogId }
                    });
                },
                AddBlogComment: function (blogComment) {
                    return $http({
                        url: "/Blogs/AddBlogComment",
                        method: "POST",
                        data: { blogComment: blogComment }
                    });
                },
               
            }
        });
})();