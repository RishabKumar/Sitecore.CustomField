; (function () {
	'use strict';
	
	// splittingHours();
	seekbarPosition();
	currentShow();
	setInterval(function(){
		seekbarPosition();
		currentShow();
		setTimeout(function(){ 
			positionShow();
		}, 500);
	}, 1000);
	
	$.ajax({
                    url: "js/program.json",
                    //force to handle it as text
                    dataType: "text",
                    success: function(data) {
                        
                        //data downloaded so we call parseJSON function 
                        //and pass downloaded data
                        var json = $.parseJSON(data);
                        //now json variable contains data in json format
                        //let's display a few items
                        $('#results').html('Plugin name: ' + json.name + '<br />Author: ' + json.author.name);
                    }
                });
	/*Position of show*/
	function positionShow(){
		var boxWidth=$('.block').width();
		var transformVal=parseFloat($('.inner-slider-show-list').css('transform').split(/[()]/)[1].split(',')[4]);
		var blockWidth= totalWidth() - ($('.slider-show-list').width());
		$('.block').each(function() {    
		   if($(this).hasClass('active')){
			boxWidth=$('.block.active').width();
			var duration= splitTime($('.block.active span.duration').text());
			var startTime= splitTime($('.block.active span.time').text());
			var indexVal=$(this).index();
			var move= showTimeBox(boxWidth,duration,startTime);
			$('.inner-slider-show-list').css({"transition-duration":"500ms","transition-timing-function":"cubic-bezier(0.12, 0.52, 0.1, 0.91)"});
			$('.inner-slider-show-list').css("transform","translateX(" + (-transformX(indexVal,move)) + "px)");	
		   }
		});
	}
	
	/*Showtime Box movement*/
	function showTimeBox(activeBoxWidth,activeDuration,activeStartTime){
		var curTime=getCurrentTime();
		var remDuration = ((curTime-activeStartTime)/activeDuration);
		var widthMovePixel = remDuration*activeBoxWidth;
		return widthMovePixel;
	}
	
	/*Get Current position of show*/
	function transformX(index,moveWidth){
		var temp=0;		
		var fullDayTime = ((23*60*60) + (59*60) + 60);
		var seekPosition=seekbarPosition();
		var t = (seekPosition/100)*($('.epg-player-info').width());
		for(var g=0; g<index;g++){ 
			temp= temp + parseInt($('.block').eq(g).width());
		}
		//console.log(moveWidth);
		temp = temp - t + index*12 + moveWidth;
		//console.log(Math.floor(temp));
		return Math.floor(temp);
	}
	
	/*Get current Time*/
	function getCurrentTime(){
		var d = new Date();
		var time = ((d.getHours()*60*60) + (d.getMinutes()*60) + d.getSeconds());
		return time;
	}
	
	/*Split time in seconds*/
	function splitTime(a){
		var splitTime=a.split(':');
		var splitTimeSec=(parseInt(splitTime[0]*60*60)+parseInt(splitTime[1]*60)+parseInt(splitTime[2]));
		return splitTimeSec;
	}
	
	/*Get current seekbar position as per current time*/
	function seekbarPosition(){
		var currentTime = getCurrentTime();
		var fullDayTime = ((23*60*60) + (59*60) + 60);
		var seekbarwidth= (currentTime/fullDayTime)*100;
		$('.progress-line').css({ left: seekbarwidth+'%'}, 'slow');
		return seekbarwidth;
	}
	
	/*Get total width of shows*/
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
	
	/*Get current show*/
	function currentShow(){
		var z,z3,z2,a,count,count2,time2,time3,duration;
		var dur=[];
		dur=$('span.duration');
		var dayname=$('span.day-name');
		var lidata=$('.week-info-list li');
		var arr=[];
		arr = $('span.time');
		count = arr.length;
		count2 = dayname.length;
		var currentTime = getCurrentTime();
		for(var i = 0; i < count; i++){
			z= arr.eq(i).text();			
			a= z.split(':');
			time2 = (parseInt(a[0]*60*60)+parseInt(a[1]*60)+parseInt(a[2]));
			var p =dur.eq(i).text();
			var q=p.split(':');
			duration = (parseInt(q[0]*60*60)+parseInt(q[1]*60)+parseInt(q[2]));
			arr.eq(i).parent().parent().css('width',(duration/20)+'px');
			time3= time2+duration;
			if((time3>currentTime) && (currentTime>time2)){
				arr.eq(i).parent().parent().addClass('active');				
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
			}
		}
	}
	
	
}());