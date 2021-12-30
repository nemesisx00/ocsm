import './Merits.css'
import React from 'react'
import EntryList from '../core/EntryList'
import Tracker from './Tracker'
import { compareStrings, normalizeClassNames } from '../core/Utilities'

const MeritKeys = Object.freeze({
	Label: 'label',
	New: 'new',
	Value: 'value'
})

const SortMerits = (a, b) => {
	let ret = b.value - a.value
	if(ret === 0)
		ret = compareStrings(a.label, b.label)
	
	return ret
}

const ThirdColumnLimit = 20

class Merit extends React.Component
{
	render()
	{
		return (<div className="entry merit">
			<input type="text" value={this.props.label} onChange={(event) => this.props.changeHandler(this.props.index, MeritKeys.Label, event.target.value)} />
			<Tracker keyWord={this.props.label} className={normalizeClassNames(`dots ${this.props.label}`)} type={Tracker.Types.Circle} max={this.props.max} value={this.props.value} changeHandler={(value) => this.props.changeHandler(this.props.index, MeritKeys.Value, value)} />
		</div>)
	}
}

class Merits extends EntryList
{
	generateEntries()
	{
		let columns = []
		let entries = []
		
		let divisor = 2 + (this.props.merits.length > ThirdColumnLimit ? 1 : 0)
		let maxRows = Math.ceil(this.props.merits.length / divisor)
		
		this.props.merits.forEach((merit, i) => {
			if(i % maxRows === 0)
			{
				if(entries.length > 0)
					columns.push((<div className="column" key={`${this.props.keyword}Column-${columns.length}`}>{entries}</div>))
				entries = []
			}
			
			entries.push(<Merit key={`merit-${i}`} index={i} label={merit.label} value={merit.value} max={this.props.max} changeHandler={(index, key, value) => this.props.changeHandler(index, key, value)} />)
		})
		//Catch the last row
		if(entries.length > 0)
			columns.push((<div className="column" key={`${this.props.keyword}Column-${columns.length}`}>{entries}</div>))
		
		return (<div className="row">{columns}</div>)
	}
	
	addNewEntry()
	{
		this.props.changeHandler(MeritKeys.New, MeritKeys.Label, '')
	}
}

Merits.Keys = MeritKeys
Merits.SortMerits = SortMerits

export default Merits
