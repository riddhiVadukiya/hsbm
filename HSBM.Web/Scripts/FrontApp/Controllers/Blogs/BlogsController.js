angular.module('FrontApp').controller('BlogsController', ['$rootScope', '$scope', '$compile', '$filter', '$timeout', 'BlogsFactory', 'CommonFactory',
function ($rootScope,$scope, $compile, $filter, $timeout, BlogsFactory, CommonFactory) {
    
    $scope.FrontBlogResponse = JSON.parse($("#hdnModelBlog").val());
    $scope.CategoryList = JSON.parse($("#hdnModelCategoryList").val());
    $scope.PopularBlogList = JSON.parse($("#hdnModelPopularBlogList").val());
    $scope.BlogArchivesList = JSON.parse($("#hdnModelBlogArchivesList").val());
    $scope.blocks = [];
    $scope.FrontBlogRequest = {};
    $scope.FrontBlogRequest.PageIndex = 1;
    $scope.BlogComments = [];
    $scope.UserComment = "";
    var blog = window.localStorage.getItem("blog");    
    
    $scope.dateFormat = 'MMMM dd, yyyy';

    $scope.GetAllBlogs = function () {
        
        $("#divBlogDetail").hide();
        $.ajax({
            url: '/Blogs/GetAllPopularBlog',
            type: 'POST',
            data: {
                //'__RequestVerificationToken': token,
                //p_SearchRequest: $scope.Search
            },
            success: function (result) {
                
                $scope.ListofBlogs = (result.Data);
                $scope.$digest();
            }
        });
    }
    $scope.GetAllBlogs();


    $scope.SetPagination = function ()
    {
        $scope.blocks = [];
        if ($scope.FrontBlogResponse != null && $scope.FrontBlogResponse != undefined && $scope.FrontBlogResponse.length > 0 && $scope.FrontBlogResponse[0].RecordsTotal>6) {
            for (i = 0; i < Math.ceil($scope.FrontBlogResponse[0].RecordsTotal / 6) ; i++) {
                $scope.blocks.push({ PageIndex: i + 1, IsSelected : false });
            }
            $scope.blocks[0].IsSelected = true;
        }
    }
    $scope.SetPagination();

    $scope.GoToSpecificPage = function () {

        var response = BlogsFactory.SearchBlogs($scope.FrontBlogRequest);
        response.then(function (successdata) {
            $scope.FrontBlogResponse = successdata.data.Data;

            angular.forEach($scope.blocks, function (block) {
                block.IsSelected = false;
            });
            $scope.blocks[$scope.FrontBlogRequest.PageIndex - 1].IsSelected = true;

        }).catch(function (data, status) {
            console.error('Error', response.status, response.data);
        }).finally(function () {
            console.log("finally finished");
        });

    }

    $scope.Previous = function () {
        if($scope.FrontBlogRequest.PageIndex - 1 <= 0)
        {
            return;
        }
        else
        {
            $scope.FrontBlogRequest.PageIndex -= 1;
            $scope.GoToSpecificPage();
        }
    }

    $scope.Next = function () {
        if ($scope.FrontBlogRequest.PageIndex + 1 > $scope.blocks.length) {
            return;
        }
        else {
            $scope.FrontBlogRequest.PageIndex += 1;
            $scope.GoToSpecificPage();
        }
    }

    $scope.searchkeypress = function (keyEvent) {
        if (keyEvent.which === 13)
        {
            $scope.SearchbyKeyword(true);
        }
    }

    $scope.SearchbyKeyword = function (bykeyword, catid) {
        if (bykeyword)
        {
            $scope.FrontBlogRequest.CategoryId = 0;
        }
        else
        {
            $scope.FrontBlogRequest.keyword = "";
            $scope.FrontBlogRequest.CategoryId = catid;
        }
        
        $scope.FrontBlogRequest.PageIndex = 1;

        var response = BlogsFactory.SearchBlogs($scope.FrontBlogRequest);
        response.then(function (successdata) {
            $scope.FrontBlogResponse = successdata.data.Data;

            angular.forEach($scope.blocks, function (block) {
                block.IsSelected = false;
            });
            $scope.SetPagination();
            $scope.blocks[$scope.FrontBlogRequest.PageIndex - 1].IsSelected = true;
            $scope.HideDetail();
        }).catch(function (data, status) {
            console.error('Error', response.status, response.data);
        })
        
    }
    $scope.SearchbyArchives = function (archivesData) {
        
        $scope.FrontBlogRequest.ArchivesData = archivesData;
        $scope.FrontBlogRequest.PageIndex = 1;

        var response = BlogsFactory.SearchBlogs($scope.FrontBlogRequest);
        response.then(function (successdata) {
            $scope.FrontBlogResponse = successdata.data.Data;

            angular.forEach($scope.blocks, function (block) {
                block.IsSelected = false;
            });
            $scope.SetPagination();
            $scope.blocks[$scope.FrontBlogRequest.PageIndex - 1].IsSelected = true;
            $scope.HideDetail();
        }).catch(function (data, status) {
            console.error('Error', response.status, response.data);
        })
    }

    $scope.ResetSearch = function () {
        $scope.FrontBlogRequest.keyword = "";
        $scope.SearchbyKeyword();
    }

    $scope.ShowDetail = function (blog) {
        
        $scope.blog = blog;
        $("#divBlogDetail").show();
        $("#divBlogList").hide();
        $("#txtComment").removeClass("redborder");
        $scope.UserComment = "";
        $("#divResultmsg").hide();

        var response = BlogsFactory.GetBlogCommentsByBlogId(blog.Id);
        response.then(function (successdata) {
            
            $scope.BlogComments = successdata.data.Data;
            
        }).catch(function (data, status) {
            console.error('Error', response.status, response.data);
        })
        //window.localStorage.setItem("blog", null);
        window.localStorage.removeItem("blog");
        
    }

    if (blog != null && blog != undefined) {
        
        $scope.ShowDetail(JSON.parse(blog));

    }

    $scope.HideDetail = function () {
        
        $("#divBlogDetail").hide();
        $("#divBlogList").show();
        $("html, body").animate({ scrollTop: 0 }, "slow");
    }

    $scope.SaveComment = function () {
        var isLoggedIn = CommonFactory.CheckUserIsLoggedIn();
        isLoggedIn.then(function (data) {

            if (!data.data) {
                $("#frmLogin").show();
                $("#loginModal").modal({ backdrop: 'static', keyboard: false });
                $("#loginModal").modal('show');
            }
            else
            {
                if ($scope.UserComment == "" || $scope.UserComment == null || $scope.UserComment == undefined) {
                    $("#txtComment").addClass("redborder");
                    return;
                }
                else {
                    $("#txtComment").removeClass("redborder");
                }
                

                $scope.blogComment = {};
                $scope.blogComment.BlogId = $scope.blog.Id;
                $scope.blogComment.Comment = $scope.UserComment;

                var response = BlogsFactory.AddBlogComment($scope.blogComment);
                response.then(function (successdata) {
                    $scope.UserComment = "";
                    $scope.res_msg = "Thank You !!!";                    
                    $("#divResultmsg").addClass("success").show();
                    $("#divResultmsg").removeClass("error").show();
                    //$scope.blog.CommentCount += 1;
                }).catch(function (data, status) {
                    $scope.res_msg = "Error in saving comment !!!";
                    $("#divResultmsg").addClass("error").show();
                    $("#divResultmsg").removeClass("success").show();
                })

                
                                             
            } 

        }).catch(function () {
            return;
        }).finally(function () { return; });        
        
    }

    

}]);



