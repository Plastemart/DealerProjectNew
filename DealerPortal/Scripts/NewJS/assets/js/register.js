
$('li.dropdown-submenu').on('click', function(event) {
      event.stopPropagation(); 
      if ($(this).hasClass('open')){
          $(this).removeClass('open');
      }else{
          $('li.dropdown-submenu').removeClass('open');
          $(this).addClass('open');
     }
  });
// JavaScript Validation For Registration Page
$('document').ready(function()
{ 		 		
		 // name validation
		  var nameregex = /^[a-zA-Z ]+$/;
		 
		 $.validator.addMethod("validname", function( value, element ) {
		     return this.optional( element ) || eregex.test( value );
		 }); 
		 
		 // valid email pattern
		 var eregex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
		 
		 $.validator.addMethod("validemail", function( value, element ) {
		     return this.optional( element ) || eregex.test( value );
		 });
		 
		 $("#register-form").validate({
					
		  rules:
		  {
				company: {
					required: true,
					validname: true,
					
				},
				branch: {
					required: true,
					validemail: true
				},
				username: {
					required: true,
					validname: true,
					minlength: 8,
					maxlength: 15
				},
				cusername: {
					required: true,
					validname: true,
					minlength: 8,
					maxlength: 15
				},
				
				password: {
					required: true,
					equalTo: '#password'
				},
		   },
		   messages:
		   {
				company: {
					required: "Please Enter Company Name",
					validname: "Enter Valid Company Name",
					  },
			    branch: {
					  required: "Please Enter Branch Name",
					  validemail: "Enter Valid Branch Name"
					   },
				username:{
					required: "Please Enter Username",
					validname: "Enter Valid Username"
					},
				cusername: {
					required: "Please Enter Username",
					validname: "Enter Valid Username"
					},	
				password:{
					required: "Please Enter your Password",
					equalTo: "Password at least have 8 characters"
					}
		   },
		   errorPlacement : function(error, element) {
			  $(element).closest('.form-group').find('.help-block').html(error.html());
		   },
		   highlight : function(element) {
			  $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
		   },
		   unhighlight: function(element, errorClass, validClass) {
			  $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
			  $(element).closest('.form-group').find('.help-block').html('');
		   },
		   
		   		submitHandler: function(form){
					
					alert('submitted');
					form.submit();
					//var url = $('#register-form').attr('action');
					//location.href=url;
					
					
					
					
					
				}
				
				
				
				
				/*submitHandler: function() 
							   { 
							   		alert("Submitted!");
									$("#register-form").resetForm(); 
							   }*/
		   
		   }); 
		   
		   
		   /*function submitForm(){
			 
			   
			   /*$('#message').slideDown(200, function(){
				   
				   $('#message').delay(3000).slideUp(100);
				   $("#register-form")[0].reset();
				   $(element).closest('.form-group').find("error").removeClass("has-success");
				    
			   });
			   
			   alert('form submitted...');
			   $("#register-form").resetForm();
			      
		   }*/
		
     
	


  $('#eventDescriptions>div').hide();
  $('#eventTitles a').click(function(){
	  $ ('.top-form').slideToggle();
    var target = $(this).attr("rel");
    $(target).slideToggle('slow');
  });

  $('#eventDescriptions a.close-btn').click(function(){
	  $ ('.top-form').slideToggle();
    $(this).parent().parent().slideToggle('slow');
  })


 $(".js-example-placeholder-single").select2({
 
});
$(".js-example-placeholder-single1").select2({
 
});



$(function () {
	
	
	$('.subnavbar').find ('li').each (function (i) {
	
		var mod = i % 3;
		
		if (mod === 2) {
			$(this).addClass ('subnavbar-open-right');
		}
		
	});
	
	
	
});
  
	
	 (function($) {
//                $(document).ready(function(){
//                    $('.column').sameheight();
//                });
            })(jQuery);
		   
		
		$(function() {
  /**
  * NAME: Bootstrap 3 Multi-Level by Johne
  * This script will active Triple level multi drop-down menus in Bootstrap 3.*
  */
  
  
  
});


$(document).ready(function(){
//                    $('.column').sameheight();
                });
		
		   
});






$(document).on('click', '#close-preview', function(){ 
    $('.image-preview').popover('hide');
    // Hover befor close the preview    
});

$(function() {
    // Create the close button
    var closebtn = $('<button/>', {
        type:"button",
        text: 'x',
        id: 'close-preview',
        style: 'font-size: initial;',
    });
    closebtn.attr("class","close pull-right");

    // Clear event
    $('.image-preview-clear').click(function(){
        $('.image-preview').attr("data-content","").popover('hide');
        $('.image-preview-filename').val("");
        $('.image-preview-clear').hide();
        $('.image-preview-input input:file').val("");
        $(".image-preview-input-title").text("Browse"); 
    }); 
    // Create the preview image
    $(".image-preview-input input:file").change(function (){     
        var img = $('<img/>', {
            id: 'dynamic',
            width:250,
            height:200
        });      
        var file = this.files[0];
        var reader = new FileReader();
        // Set preview image into the popover data-content
        reader.onload = function (e) {
            $(".image-preview-input-title").text("Change");
            $(".image-preview-clear").show();
            $(".image-preview-filename").val(file.name);
        }        
        reader.readAsDataURL(file);
    });  
});






