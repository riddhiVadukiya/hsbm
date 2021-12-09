
$(document).ready(function() {
    
	//Side Menu Click Effect
	$('.ripple').materialripple();
	
	//Custome Scroll bar
	$(window).load(function(){
		$('.custom-scroll').mCustomScrollbar({
			theme:"dark"
		});
	});
	
	$('.nav-left i.fa-bars').click(function() {
		$('body').toggleClass('nav-min');
	});
	
});