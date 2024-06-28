<script>
    const joinGame = ()=>
    {

    }

    // build-in guid
    
</script>

<div class="page">

    <h1>XO Game</h1>

    <div class="container">
        <button id="start" on:click={()=>joinGame()}>
            Start Playing
        </button>
        <button id="exit">
            Exit Game
        </button>
    </div>

</div>


<style>

    h1
    {
        width: 331px;
        margin: 0px;
        padding: 0px;
        color: #fff;
        font-size: 76px;
        font-style: normal;
        font-weight: 500;
        line-height: normal;
    }

    .page
    {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        width: 100%;
        height: 100vh;
    }

    .container {
        display: flex;
        flex-direction: column;
        gap:23px;
        width: 362px;
        height: 362px;
        flex-shrink: 0;
        border-radius: 40px;
        background: #aaa;
        align-items: center;
        justify-content: center;
        padding: 0px;
    }

    .container button {
        
        width: 320px;
        height: 68px;
        color: #FFF;
        font-size: 34px;
        font-style: normal;
        font-weight: 500;
        line-height: normal;
        
    }
        
    #start {    
        border-radius: 100px 100px 30px 30px;
    }

    #exit {
        border-radius: 30px 30px 100px 100px;
    }
</style>