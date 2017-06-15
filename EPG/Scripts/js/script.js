; (function () {
	'use strict';
	
	getTimeData();
	setInterval(function(){ 
	//$('.block').removeClass('active');
		getTimeData();
	}, 3000);
	
	
	setTimeout(function(){ 
		var boxWidth=$('.block').width();
		var transformVal=parseFloat($('.inner-slider-show-list').css('transform').split(/[()]/)[1].split(',')[4]);
		var blockWidth= totalWidth() - ($('.slider-show-list').width());
		$('.block').each(function() {    
		   if($(this).hasClass('active')){
			boxWidth=$('.block.active').width();
			var indexVal=$(this).index();
			seekbarMovement(indexVal);
			$('.inner-slider-show-list').css({"transition-duration":"500ms","transition-timing-function":"cubic-bezier(0.12, 0.52, 0.1, 0.91)"});
			$('.inner-slider-show-list').css("transform","translateX(" + (-transformX(indexVal)) + "px)");	
			console.log(transformX(indexVal));
		   }
		});
		
		$('button.left-btn').click(function(){
			transformVal=parseFloat($('.inner-slider-show-list').css('transform').split(/[()]/)[1].split(',')[4]);
			$('.inner-slider-show-list').css({"transition-duration":"500ms","transition-timing-function":"cubic-bezier(0.12, 0.52, 0.1, 0.91)"});
			if(transformVal<0){
				$('.inner-slider-show-list').css("transform","translateX(" + (transformVal+ (boxWidth+4)) + "px)");
			}
		});
		$('button.right-btn').click(function(){	
			transformVal=parseFloat($('.inner-slider-show-list').css('transform').split(/[()]/)[1].split(',')[4]);
			$('.inner-slider-show-list').css({"transition-duration":"500ms","transition-timing-function":"cubic-bezier(0.12, 0.52, 0.1, 0.91)"});
			
			if((-transformVal) < blockWidth){
				$('.inner-slider-show-list').css("transform","translateX(" + (transformVal- (boxWidth+4)) + "px)");
			}
		});
	}, 500);
	
	
	function seekbarMovement(index){		
		var seekbarwidth= (seekX(index)/totalWidth())*100;
		//$('.seekbar > span').css('width',seekbarwidth+'%');
		$('.progress-line').animate({ left: seekbarwidth+'%'}, 'slow');
	}
	function seekX(index){
		var temp=0;
		for(var g=0; g<index;g++){ 
			temp= temp + parseInt($('.block').eq(g).width());
			if(g===index-1){
				temp=temp+(parseInt($('.block').eq(g).width())/2)
			}
		}
		return temp;
	}
	
	function transformX(index){
		var temp=0;
		for(var g=0; g<index;g++){ 
			temp= temp + parseInt($('.block').eq(g).width());
		}
		return temp;
	}
	
	function totalWidth(){
		var temp=0;
		for(var g=0; g<$('.block').length;g++){ 
			temp= temp + parseInt($('.block').eq(g).width());
			if(g===$('.block').length-1){
				temp=temp+(parseInt($('.block').eq(g).width())/2)
			}
		}
		return temp;
	}
	
	
	function getTimeData(){
		var z,z3,z2,a,count,count2,time2,time3,duration;
		var dur=[];
		dur=$('span.duration');
		
		//var duration=30*60;		
		var dayname=$('span.day-name');
		var lidata=$('.week-info-list li');
		var d = new Date();
		var time = ((d.getHours()*60*60) + (d.getMinutes()*60) + d.getSeconds());
		var arr=[];
		arr = $('span.time');
		count = arr.length;
		count2 = dayname.length;
		for(var i = 0; i < count; i++){
			z= arr.eq(i).text();			
			a= z.split(':');
			time2 = (parseInt(a[0]*60*60)+parseInt(a[1]*60)+parseInt(a[2]));
			var p =dur.eq(i).text();
			var q=p.split(':');
			duration = (parseInt(q[0]*60*60)+parseInt(q[1]*60)+parseInt(q[2]));
			arr.eq(i).parent().parent().css('width',(duration/20)+'px');
			time3= time2+duration;
			//arr.eq(i).parent().parent().removeClass('active');
			if((time3>time) && (time>time2)){
				arr.eq(i).parent().parent().addClass('active');
				seekbarMovement(i);
				for(var j = 0; j < count2; j++){
					z2=dayname.eq(i).text();
					z3=lidata.eq(j).text();
					if(z3===z2){
						lidata.eq(j).addClass('active');
					}else{
						lidata.eq(j).removeClass('active');
					}
				}
			}else{
				arr.eq(i).parent().parent().removeClass('active');
				//$('.seekbar > span').animate({ width: '0'}, 'slow');
			}
		}
	}
	
}());