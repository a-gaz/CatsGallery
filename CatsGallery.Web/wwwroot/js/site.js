class CatGallery {
    constructor() {
        this.photoLoadOffset = 1;
        this.currentIndex = 1;
        this.cats = [];
        this.init();
    }

    async init() {
        const response = await fetch('/Gallery/Init');
        const data = await response.json();

        this.cats = [
            { imageBytes: data.prevCat },
            { imageBytes: data.currCat },
            { imageBytes: data.nextCat }
        ];
        
        this.updateView();
    }

    async prevCat() {
        this.currentIndex = (this.currentIndex - 1 + this.cats.length) % this.cats.length;
        
        this.updateView();
    }

    async nextCat() {
        this.currentIndex = (this.currentIndex + 1) % this.cats.length;
        console.log("this.currentIndex " + this.currentIndex);
        console.log("this.this.cats.length " + (this.cats.length - 1));
        if(this.currentIndex === this.cats.length - 1 - this.photoLoadOffset) {
            const response = await fetch('/Gallery/Next');
            const data = await response.json();
            this.cats.push({ imageBytes: data.nextCat } );
        }

        this.updateView();
    }

    updateView() {
        const prevIndex = (this.currentIndex - 1 + this.cats.length) % this.cats.length;
        const nextIndex = (this.currentIndex + 1) % this.cats.length;

        for (let i = 1; i < this.cats.length; i++) {
            console.log(this.cats[i])
        }
        
        document.getElementById('prev-cat').src = `data:image/jpeg;base64,${this.cats[prevIndex].imageBytes}`;
        document.getElementById('curr-cat').src = `data:image/jpeg;base64,${this.cats[this.currentIndex].imageBytes}`;
        document.getElementById('next-cat').src = `data:image/jpeg;base64,${this.cats[nextIndex].imageBytes}`;
    }
}

document.addEventListener('DOMContentLoaded', () => {
    window.gallery = new CatGallery();

    document.querySelector('.btn-prev').addEventListener('click', async () => {
        await window.gallery.prevCat();
    });

    document.querySelector('.btn-next').addEventListener('click', async () => {
        await window.gallery.nextCat();
    });
});

