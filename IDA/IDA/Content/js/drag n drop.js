const cards = document.querySelectorAll('.card');
const list_group = document.querySelectorAll('.list-group');

let dragedCard = null;

for (let i =0; i = cards.length; i++) {
	
	const card = cards[i];

	card.AddEventListener('dragstart', function (){
		dragedCard = card;
		setTimeout (function(){
			card.style.display = 'none';
		},0);
	});

	card.AddEventListener('dragend',function(){

		setTimeout(function(){
			dragedCard.style.display = 'block';
			dragedCard = null;
		},0);

	})

	for(let j = 0; i< list_group.length)
	{
			const list = list_group[j];
			list.AddEventListener('dragover', function(e){
				e.preventDefault();
			});
			list.AddEventListener('dragenter', function(e){
				e.preventDefault();
				this.style.backgroundColor = 'rgba(0, 0, 0, 0, 0.2)';
			});
			list.AddEventListener('dragleave', function(){
				this.style.backgroundColor = 'rgba(0, 0, 0, 0, 0.1)';
			});
			list.AddEventListener('drop', function(e){
				console.log('drop');
				this.append(dragedCard);
				this.style.backgroundColor = 'rgba(0, 0, 0, 0, 0.1)';
			});

	}
}