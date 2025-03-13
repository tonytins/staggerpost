# Publish Times

This is a very simple console application that generates a list of times to publish news articles within a randomized 2-3 hour delay while avoiding time conflicts within a 30-minute window.

## Exporting

Once it generates a list of times, you can retry or export the times. If you export, you're requested to select from a number of topics or default to none if no selection is made. Upon export, you can start over or exit.

 Choice selection is based on the Y/N keys. ``Enter`` works the same as ``Y`` while pressing any other key is the equivalent of ``N``. You can make mistakes, but it expects failure.

An optional ``config.toml`` file allows for further customization of file name, directory, and topics.

## Background

A while back, I [found a tool](https://schedule.lemmings.world) to schedule articles on Lemmy. I've been posting within a few hours apart at random minutes and I wanted to something decide that for me. I had AI write the base algorithm, everything else is my own touches.

## License

I hereby waive this project under the public domain - see [UNLICENSE](UNLICENSE) for details.
